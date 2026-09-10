# BitChord for Windows

This folder contains the native WinUI 3 companion for BitChord. It is intentionally
separate from the Android Gradle project so both clients can evolve without
coupling their build systems.

## Requirements

- Windows 10 version 1809 or later
- Visual Studio 2022 17.10 or later
- **.NET Desktop Development** and **Windows App SDK** workloads

## Run

Open `BitChord.sln`, select `BitChord.WinUI`, choose `x64`, and press **F5**.
The app uses Windows 11 Mica/Acrylic-compatible surfaces, adaptive navigation,
keyboard-friendly controls, a persistent mini-player, and reduced-motion settings.

## Build the installer

Select the `BitChord.Package` project and build `Release | x64`. Visual Studio
places the unsigned MSIX bundle under `windows\artifacts`. For distribution,
configure a certificate in the project's **Package** properties and enable
`AppxPackageSigningEnabled`.

The package project links the existing repository `Logo.png` instead of
duplicating the Android artwork. No credentials or service keys are included.

## Build automatically with GitHub Actions

The `Windows companion` workflow runs on pushes, pull requests, and manual
dispatches. It restores and builds the WinUI project, creates an unsigned MSIX
bundle on `windows-2022`, and uploads the package as a 14-day GitHub Actions
artifact. Open the workflow run in GitHub and download
`bitchord-windows-msix-<run-number>` from the **Artifacts** section to test it
before the pull request is merged.
