name: check

on:
  push:
    branches:
      - master
  pull_request:
    branches:
      - master

jobs:
  build:
    runs-on: ubuntu-18.04
    steps:
      - uses: actions/checkout@master
      - name: Check password
        uses: AlicanAkkus/pinder-action@0.1
