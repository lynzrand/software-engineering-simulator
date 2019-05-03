using System;

namespace Sesim.Models
{
    #region models
    public interface ISoftwareDevStage
    {
        string name { get; }

    }

    public interface ISoftwareDevModel
    {
        ISoftwareDevStage[] stages { get; }
    }
    #endregion


    #region Waterfall Model
    public class WaterfallModel : ISoftwareDevModel
    {
        public ISoftwareDevStage[] stages => throw new NotImplementedException();
    }
    #endregion



}