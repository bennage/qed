﻿using System;
using System.Diagnostics;

namespace qed
{
    public static partial class Functions
    {
        public static bool FetchRepository(
            Build build,
            string repositoryDirectory,
            Action<string> log)
        {
            return FetchRepository(
                build,
                repositoryDirectory,
                log,
                CreateProcess,
                CreateFetchRefspec,
                RunProcess);
        }

        internal static bool FetchRepository(
            Build build,
            string repositoryDirectory,
            Action<string> log,
            Func<string, string, string, Process> createProcess,
            Func<string, string> createFetchRefspec,
            Func<Process, Action<string>, int> runProcess)
        {
            log("STEP: Fetching repository.");

            Func<bool> step = () =>
            {
                using (var process = createProcess(
                    "git.exe",
                    String.Concat("fetch origin ", createFetchRefspec(build.Ref)), repositoryDirectory))
                {
                    return runProcess(process, log) == 0;
                }
            };

            return RunStep(step, log);
        }
    }
}
