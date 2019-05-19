using System;
using Sesim.Models;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using Sesim.Helpers.UI;

namespace Sesim.Game.Controllers.MainGame
{
    public class StatusPanelController : MonoBehaviour
    {
        public CompanyActionController src;
        public Text timeDisplayer;
        public Text warpDisplayer;
        public Text fundDisplayer;
        public Text reputationDisplayer;
        public Text employeeCountDisplayer;
        public Text contractCountDisplayer;
        public GameObject notificationDisplayer;

        public static Base16ColorScheme scheme = Base16ColorScheme.MaterialPaleNight;

        public void Start()
        {
            src = src ?? gameObject.GetComponentInParent<CompanyActionController>();
            src.AfterCompanyUpdate += this.PassiveUpdate;
        }

        public void PassiveUpdate(CompanyActionController controller)
        {
            timeDisplayer.text = FormatTimeDisplay(src.company.ut, src.timeWarpMultiplier);
            warpDisplayer.text = FormatTimeWarp(src.timeWarpMultiplier);
            fundDisplayer.text = FormatFund(src.company.fund);
            reputationDisplayer.text = FormatReputation(src.company.reputation);
        }

        public static string FormatReputation(float reputation)
        {
            return reputation.ToString("0000.00");
        }

        public static string FormatFund(decimal fund)
        {
            return fund.ToString("000 000 000 000.00");
        }

        public static string FormatTimeDisplay(double ut, float warpSpeed)
        {
            var time = Company.UtToTime(ut);
            return String.Format("Day<b>{0:0000}</b> {1:00}:{2:00}<b>@{3:#}x</b>", time.days, time.hours, time.minutes, warpSpeed);
        }

        public static string FormatTimeWarp(float warpSpeed)
        {
            const int warpLevelCount = 8;
            var warpLevel = Mathf.FloorToInt(Mathf.Clamp(Mathf.Log(warpSpeed, 2), 0, warpLevelCount - 1));
            var sb = new StringBuilder();
            sb.Append(ConsoleHelper.ColorOpeningTag(scheme.base07));
            sb.Append('=', warpLevel);
            sb.Append('>');
            sb.Append(ConsoleHelper.ColorClosingTag());
            sb.Append(ConsoleHelper.ColorOpeningTag(scheme.base04));
            sb.Append('>', warpLevelCount - warpLevel - 1);
            sb.Append(ConsoleHelper.ColorClosingTag());
            return sb.ToString();
        }
    }
}
