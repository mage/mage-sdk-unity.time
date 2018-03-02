using Newtonsoft.Json.Linq;
using System.Collections;
using Wizcorp.MageSDK.MageClient;

namespace Wizcorp.MageSDK.Time
{
	public class Time : Module<Time>
	{
		public Timer Server = new Timer();
		public Timer Client = new Timer();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator SetupTask() {
			IEnumerator wait = Synchronize();
			while (wait.MoveNext()) {
				yield return null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double Offset {
			get { return Client.OffsetMSec - Server.OffsetMSec; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="timestamp"></param>
		/// <param name="msecOut"></param>
		/// <returns></returns>
		public double ServerToClientTime(double timestamp, bool msecOut = false) {
			return Client.Translate(timestamp, msecOut);
		}

		/// <summary>
		/// 
		/// </summary>
		public double ClientNow(bool msecOut = false) {
			return Client.Now(msecOut);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator Synchronize() {
			// Retrieve 
			UserCommandStatus status = Command("sync", new JObject {
				{ "clientTime", Timer.DateNowMSec() }
			});
			while (!status.Done) {
				yield return null;
			}

			// Handle error
			if (status.Error != null) {
				throw status.Error;
			}

			// Get time configuration
			JToken config = status.Result;

			// Configure server timer
			Server.Configure(
				(double)config["timer"]["offset"] - (double)config["delta"],
				(double)config["timer"]["accelerationFactor"],
				(double)config["timer"]["startAt"] + (double)config["delta"]
			);

			// Configure client timer
			Client.Configure(
				(double)config["timer"]["offset"],
				(double)config["timer"]["accelerationFactor"],
				(double)config["timer"]["startAt"] + (double)config["delta"]
			);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="timestamp"></param>
		/// <param name="msecOut"></param>
		/// <returns></returns>
		public double Translate(double timestamp, bool msecOut = false) {
			return Server.Translate(timestamp, msecOut);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msecOut"></param>
		/// <returns></returns>
		public double Now(bool msecOut = false) {
			return Server.Now(msecOut);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public double MSec() {
			return Server.MSec();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public double Sec() {
			return Server.Sec();
		}
	}
}
