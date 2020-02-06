﻿#region license
// Copyright (C) 2020 ClassicUO Development Community on Github
// 
// This project is an alternative client for the game Ultima Online.
// The goal of this is to develop a lightweight client considering
// new technologies.
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion


namespace ClassicUO.Utility.Logging
{
    internal class Log
    {
        private static Logger _logger;

        public static void Start(LogTypes logTypes, LogFile logFile = null)
        {
            _logger = _logger ?? new Logger
            {
                LogTypes = logTypes
            };
            _logger.Start(logFile);
        }

        public static void Stop()
        {
            _logger?.Stop();
            _logger = null;
        }

        public static void Resume(LogTypes logTypes)
        {
            _logger.LogTypes = logTypes;
        }

        public static void Pause()
        {
            _logger.LogTypes = LogTypes.None;
        }

        public static void Debug(string text)
        {
            _logger.Message(LogTypes.Debug, text);
        }

        public static void Info(string text)
        {
            _logger.Message(LogTypes.Info, text);
        }

        public static void Trace(string text)
        {
            _logger.Message(LogTypes.Trace, text);
        }

        public static void Warn(string text)
        {
            _logger.Message(LogTypes.Warning, text);
        }

        public static void Error(string text)
        {
            _logger.Message(LogTypes.Error, text);
        }

        public static void Panic(string text)
        {
            _logger.Message(LogTypes.Error, text);
        }

        public static void NewLine()
        {
            _logger.NewLine();
        }

        public static void Clear()
        {
            _logger.Clear();
        }

        public static void PushIndent()
        {
            _logger.PushIndent();
        }

        public static void PopIndent()
        {
            _logger.PopIndent();
        }
    }
}