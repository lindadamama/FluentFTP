﻿using System;
using System.Linq;
using FluentFTP.Helpers;
using FluentFTP.Client.Modules;
using System.Threading;
using System.Threading.Tasks;
using FluentFTP.Exceptions;

namespace FluentFTP {
	public partial class AsyncFtpClient {

		/// <summary>
		/// Sets the working directory on the server asynchronously
		/// </summary>
		/// <param name="path">The directory to change to</param>
		/// <param name="token">The token that can be used to cancel the entire process</param>
		public async Task SetWorkingDirectory(string path, CancellationToken token = default(CancellationToken)) {

			path = path.GetFtpPath();

			LogFunction(nameof(SetWorkingDirectory), new object[] { path });

			FtpReply reply;

			// exit if invalid path
			if (path is "." or "./") {
				return;
			}

			// If PreserveTrailingSlashCmdList enabled for CWD... but: Don't do it for root dir and any
			// directories that already end with a slash (which shouldn't happen, but let's be safe)
			if (Config.PreserveTrailingSlashCmdList != null && Config.PreserveTrailingSlashCmdList.Contains("CWD") && !path.EndsWith("/")) {
				path += "/";
			}

			// modify working dir
			if (!(reply = await Execute("CWD " + path, token)).Success) {
				throw new FtpCommandException(reply);
			}

		}

	}
}
