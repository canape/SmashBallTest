using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface ICourtsManager
{
    public CourtData GetCourtDataById(int courtId);
    public Court GetCourtById(int courtId);
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
}
