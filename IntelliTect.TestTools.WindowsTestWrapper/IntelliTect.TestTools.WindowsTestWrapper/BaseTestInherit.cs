﻿using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntelliTect.TestTools.WindowsTestWrapper
{
    /// <summary>
    /// Inherit from this to create an application-specific base test class with generic settings to further inherit test classes from
    /// </summary>
    [CodedUITest]
    public abstract class BaseTestInherit
    {
        /// <summary>
        /// Used in the DesktopApplication.LaunchApplication method
        /// If overriding [TestInitialize] BaseTestInitialize(),
        /// do not forget to add a call to DesktopControls.LaunchApplication(ApplicationLocation) to your own test initialize method
        /// </summary>
        public abstract string ApplicationLocation { get; }

        /// <summary>
        /// Use TestInitialize to run code before running each test 
        /// </summary>
        [TestInitialize]
        public void BaseTestInitialize()
        {
            Debug.Write( "Default test initialize start" );
            DesktopControls.LaunchApplication(ApplicationLocation);

            Playback.PlaybackSettings.WaitForReadyLevel = WaitForReadyLevel.Disabled;
            Playback.PlaybackSettings.MaximumRetryCount = 5;
            Playback.PlaybackSettings.ShouldSearchFailFast = true;
            Playback.PlaybackSettings.DelayBetweenActions = 100;
            Playback.PlaybackSettings.SearchTimeout = 1000;
            Playback.PlaybackSettings.MatchExactHierarchy = false;

            //Tip from Stack Overflow to remove and then re-add error handling to get it to initialize properly
            //Don't know why this works, but it did when Mike Curn checked using VS2010.
            Playback.PlaybackError -= PlaybackErrorHandler;
            Playback.PlaybackError += PlaybackErrorHandler;
            Debug.Write("Default test initialize finished");
        }
        /// <summary>
        /// Use TestCleanup to run code after each test has run
        /// </summary>
        [TestCleanup()]
        public void BaseTestCleanup()
        {

        }

        private static void PlaybackErrorHandler(object sender, PlaybackErrorEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            e.Result = PlaybackErrorOptions.Retry;
        }
    }
}