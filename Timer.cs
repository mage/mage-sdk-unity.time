using System;

namespace Wizcorp.MageSDK.Time
{
	public class Timer
	{
		public double OffsetMSec { get; private set; }
		public double AccelerationFactor { get; private set; }
		public double StartAt { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static double DateNowMSec() {
			return Math.Floor(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
		}

		/// <summary>
		/// 
		/// </summary>
		public Timer() {
			OffsetMSec = 0;
			AccelerationFactor = 1;
			StartAt = DateNowMSec();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="offsetMSec"></param>
		/// <param name="accelerationFactor"></param>
		/// <param name="startAt"></param>
		public void Configure(double offsetMSec = 0, double accelerationFactor = 1, double? startAt = null) {
			OffsetMSec = offsetMSec;
			AccelerationFactor = accelerationFactor;
			StartAt = (startAt != null) ? (double)startAt : DateNowMSec();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="timestamp"></param>
		/// <param name="msecOut"></param>
		/// <returns></returns>
		public double Translate(double timestamp, bool msecOut = false) {
			double now = timestamp + OffsetMSec;

			if (AccelerationFactor != 1 && timestamp >= StartAt) {
				double msecPassed = (timestamp - StartAt) * AccelerationFactor;
				now = StartAt + OffsetMSec + msecPassed;
			}

			return msecOut ? now : Math.Floor(now / 1000);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msecOut"></param>
		/// <returns></returns>
		public double Now(bool msecOut = false) {
			return Translate(DateNowMSec(), msecOut);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public double Sec() {
			return Translate(DateNowMSec());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public double MSec() {
			return Translate(DateNowMSec(), true);
		}
	}
}
