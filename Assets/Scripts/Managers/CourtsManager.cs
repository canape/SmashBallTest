using UnityEngine;
using System.Linq;
using SmashBallTest.Courts;
using SmashBallTest.ScriptableObjects;

namespace SmashBallTest.Managers
{
    public interface ICourtsManager
    {
        public CourtData GetCourtDataById(int courtId);
        public Court GetCourtById(int courtId);
        public Ball GetBallByCourtId(int courtId);
    }

    public class CourtsManager : ICourtsManager
    {
        private readonly CourtsData courtsData;

        public CourtsManager(CourtsData courtsData)
        {
            this.courtsData = courtsData;
        }

        public CourtData GetCourtDataById(int courtId)
        {
            return courtsData.Datas.FirstOrDefault(hero => hero.CourtId == courtId);
        }

        public Court GetCourtById(int courtId)
        {
            var courtData = GetCourtDataById(courtId);
            if (courtData == null)
            {
                Debug.LogError($"Cannot get the courtData for the court {courtId}");
                return null;
            }

            return courtData.GetCourt();
        }

        public Ball GetBallByCourtId(int courtId)
        {
            var courtData = GetCourtDataById(courtId);
            if (courtData == null)
            {
                Debug.LogError($"Cannot get the courtData for the court {courtId}");
                return null;
            }

            return courtData.GetBall();
        }
    }
}
