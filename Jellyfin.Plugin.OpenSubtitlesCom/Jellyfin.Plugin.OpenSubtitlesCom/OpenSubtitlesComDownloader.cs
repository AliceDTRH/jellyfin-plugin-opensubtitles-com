using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common.Net;
using System.Linq;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Configuration;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Notifications;
using MediaBrowser.Model.Globalization;
using MediaBrowser.Model.IO;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;
using MediaBrowser.Controller.Notifications;

namespace Jellyfin.Plugin.OpenSubtitlesCom
{
    public class OpenSubtitlesComDownloader : ISubtitleProvider
    {
        public OpenSubtitlesComDownloader(INotificationManager notificationManager, IUserManager userManager, ILogger<OpenSubtitlesComDownloader> logger, IHttpClient httpClient, IServerConfigurationManager config, IJsonSerializer json, IFileSystem fileSystem, ILocalizationManager localizationManager)
        {
            _notification = notificationManager;
            _user = userManager;
            _logger = logger;
            _httpClient = httpClient;

            _config = config;
            _json = json;
            _fileSystem = fileSystem;
            _localizationManager = localizationManager;
        }

        public int Order => 2;

        public string Name => "OpenSubtitles.com";

        private const bool IFIGUREDOUTHOWNOTIFICATIONWORK = false;

        private readonly INotificationManager _notification;
        private readonly IUserManager _user;
        private readonly ILogger<OpenSubtitlesComDownloader> _logger;
        private readonly IHttpClient _httpClient;
        private readonly IServerConfigurationManager _config;
        private readonly IJsonSerializer _json;
        private readonly IFileSystem _fileSystem;
        private readonly MediaBrowser.Model.Globalization.ILocalizationManager _localizationManager;

        public IEnumerable<VideoContentType> SupportedMediaTypes => new[] { VideoContentType.Episode, VideoContentType.Movie };

        public async Task<SubtitleResponse> GetSubtitles(string id, CancellationToken cancellationToken)
        {
            if (id == "PleaseSetupThePlugin") { return new SubtitleResponse(); }
            _logger.LogDebug($"GetSubtitles({id})");

            if (string.IsNullOrWhiteSpace(Plugin.Instance.Configuration.Username) && string.IsNullOrWhiteSpace(Plugin.Instance.Configuration.Password))
            {
                _logger.LogWarning("Please configure an username and password before attempting to download subtitles.");
#pragma warning disable CS0162 // Yes, this code is unreachable. If you see this and know why this code doesn't work let me know.
                if (IFIGUREDOUTHOWNOTIFICATIONWORK)
                {
                    _ = _notification.SendNotification(new NotificationRequest()

                    {
                        Date = System.DateTime.Now.AddSeconds(1),
                        Description = "Please configure a username and password before attempting to download subtitles.",
                        Level = NotificationLevel.Error,
                        Name = "OpenSubtitles.com plugin not setup",
                        Url = "",
                        NotificationType = "PluginError",
                        UserIds = _user.Users.Select(i => i.Id).ToArray()
                    }, CancellationToken.None);
                }
#pragma warning restore CS0162 // Unreachable code detected
                return new SubtitleResponse();
            }

            var api = new OpenSubtitlesComApi.Api(Plugin.Instance.Configuration.ApiKey);
            OpenSubtitlesComApi.AuthenticationApi.TryLogin(api, Plugin.Instance.Configuration.Username, Plugin.Instance.Configuration.Password);
            var downloadapi = new OpenSubtitlesComApi.DownloadApi(api, id);

            if (await downloadapi.PerformSubtitleDownloadRequest(cancellationToken))
            {
                // We need to download the subtitle regardless of the location.
                // Sadly we don't decide what protocol is used and people want their subs.
#pragma warning disable SecurityIntelliSenseCS
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(downloadapi.Url);
#pragma warning restore SecurityIntelliSenseCS
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                return new SubtitleResponse()
                {
                    Format = "srt",
                    Language = "en",
                    Stream = resp.GetResponseStream()
                };
            }
            else
            {
                throw new MediaBrowser.Common.Extensions.ResourceNotFoundException();
            }
        }

        public async Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Search({request.Name})");
            var api = new OpenSubtitlesComApi.Api(Plugin.Instance.Configuration.ApiKey);
            OpenSubtitlesComApi.SubtitleResponse subtitleResponse = await OpenSubtitlesComApi.SubtitleApi.Search(api, "all", request.MediaPath, "en");
            var remoteSubtitleInfos = new List<RemoteSubtitleInfo>();
            if (string.IsNullOrWhiteSpace(Plugin.Instance.Configuration.Username) && string.IsNullOrWhiteSpace(Plugin.Instance.Configuration.Password))
            {
                remoteSubtitleInfos.Add(new RemoteSubtitleInfo()
                {
                    Name = "Please setup the plugin",
                    Format = "srt",
                    Author = "Plugin",
                    Comment = "Downloads won't work until this plugin is setup",
                    CommunityRating = float.MaxValue,
                    DateCreated = System.DateTime.Now,
                    Id = "PleaseSetupThePlugin",
                    ProviderName = Name,
                    DownloadCount = int.MaxValue,
                    IsHashMatch = false,
                    ThreeLetterISOLanguageName = request.Language
                });
            }
            foreach (var item in subtitleResponse.data)
            {
                remoteSubtitleInfos.Add(new RemoteSubtitleInfo()
                {
                    Name = item.attributes.files[0].file_name,
                    Format = item.attributes.format,
                    Author = item.attributes.uploader.name,
                    Comment = item.attributes.comments,
                    CommunityRating = item.attributes.votes,
                    DateCreated = item.attributes.upload_date,
                    Id = item.attributes.files[0].file_id.ToString(),
                    ProviderName = Name,
                    DownloadCount = item.attributes.new_download_count,
                    IsHashMatch = false,
                    ThreeLetterISOLanguageName = item.attributes.language
                });
            }

            return remoteSubtitleInfos;
        }
    }
}
