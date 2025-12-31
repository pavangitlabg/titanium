/*
    Services/Extensions.cs

    extension methods
*/

using Data.DBModels ;
using Data.Models ;

namespace Services
{
    static class Extensions
    {
        #region TimeDimensionDB

        /// <summary>Return as a model.</summary>
        /// <param name="entity">DB model</param>
        /// <returns>model</returns>

        public static TimeDimensionModel ToTimeDimensionModel( this TimeDimensionDB entity )
        {
            return new TimeDimensionModel
            {
                TIME_ID = entity.TIME_ID ,
                YEAR = entity.YEAR ,
                QUARTER = entity.QUARTER ,
                MONTH = entity.MONTH ,
                WEEK = entity.WEEK ,
                PERIOD_NAME_OLD = entity.PERIOD_NAME_OLD ,
                PERIOD_END_DATE = entity.PERIOD_END_DATE ,
                NUMBER_OF_WEEKS_IN_PERIOD = entity.NUMBER_OF_WEEKS_IN_PERIOD ,
                CUMULATIVE_NUMBER_OF_CALENDAR_DAYS = entity.CUMULATIVE_NUMBER_OF_CALENDAR_DAYS ,
                CUMULATIVE_WEEKS = entity.CUMULATIVE_WEEKS ,
                NUMBER_OF_CALENDAR_DAYS = entity.NUMBER_OF_CALENDAR_DAYS ,
                QUARTER_IN_FINANCIAL_YEAR = entity.QUARTER_IN_FINANCIAL_YEAR ,
                MONTH_IN_FINANCIAL_YEAR = entity.MONTH_IN_FINANCIAL_YEAR ,
                ORDINAL_MONTH_OF_QUARTER = entity.ORDINAL_MONTH_OF_QUARTER ,
                ORDINAL_WEEK_OF_MONTH = entity.ORDINAL_WEEK_OF_MONTH ,
                WEEK_POSITION_IN_MONTH = entity.WEEK_POSITION_IN_MONTH ,
                PERIOD_GRANULARITY = entity.PERIOD_GRANULARITY ,
                PERIOD_SHORT_LEGEND = entity.PERIOD_SHORT_LEGEND ,
                PERIOD_LONG_LEGEND = entity.PERIOD_LONG_LEGEND ,
                MONTH_DATE = entity.MONTH_DATE ,
                PERIOD_NAME = entity.PERIOD_NAME ,
                FINANCIAL_YEAR = entity.FINANCIAL_YEAR
            } ;
        }

        # endregion
    }
}