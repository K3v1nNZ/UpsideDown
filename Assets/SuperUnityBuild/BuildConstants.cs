using System;

// This file is auto-generated. Do not modify or move this file.

namespace SuperUnityBuild.Generated
{
    public enum ReleaseType
    {
        None,
        Debug,
        Shipping,
    }

    public enum Platform
    {
        None,
        Windows,
        Linux,
    }

    public enum ScriptingBackend
    {
        None,
        IL2CPP,
    }

    public enum Target
    {
        None,
        Player,
    }

    public enum Distribution
    {
        None,
    }

    public static class BuildConstants
    {
        public static readonly DateTime buildDate = new DateTime(638786140905942351);
        public const string version = "1.0.0.24";
        public const int buildCounter = 0;
        public const ReleaseType releaseType = ReleaseType.Shipping;
        public const Platform platform = Platform.Linux;
        public const ScriptingBackend scriptingBackend = ScriptingBackend.IL2CPP;
        public const Target target = Target.Player;
        public const Distribution distribution = Distribution.None;
    }
}

