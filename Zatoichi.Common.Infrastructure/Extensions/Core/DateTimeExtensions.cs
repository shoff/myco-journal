namespace Zatoichi.Common.Infrastructure.Extensions.Core
{
    using System;

    public static class DateTimeExtensions
    {
        public static int BirthdayToAge(this DateTime dateTime)
        {
            var today = DateTime.Today;
            var age = today.Year - dateTime.Year;

            if (age > 0)
            {
                age -= Convert.ToInt32(today.Date < dateTime.Date.AddYears(age));
            }

            return age;
        }

        public static int BirthdayToAge(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return 0;
            }

            var today = DateTime.Today;
            var age = today.Year - dateTime.Value.Year;

            if (age > 0)
            {
                age -= Convert.ToInt32(today.Date < dateTime.Value.Date.AddYears(age));
            }

            return age;
        }

        public static bool DateOnly(this DateTime first, DateTime second)
        {
            return first.Year == second.Year && first.Day == second.Day && first.Month == second.Month;
        }

        public static bool DateOnly(this DateTime? first, DateTime second)
        {
            if (!first.HasValue)
            {
                return false;
            }

            return first.Value.Year == second.Year && first.Value.Day == second.Day &&
                   first.Value.Month == second.Month;
        }

        public static bool DateOnly(this DateTime first, DateTime? second)
        {
            if (!second.HasValue)
            {
                return false;
            }

            return first.Year == second.Value.Year && first.Day == second.Value.Day &&
                   first.Month == second.Value.Month;
        }


        public static bool DateOnly(this DateTime? first, DateTime? second)
        {
            if (!second.HasValue && !first.HasValue)
            {
                return true;
            }

            if (!second.HasValue || !first.HasValue)
            {
                return false;
            }

            return first.Value.Year == second.Value.Year && first.Value.Day == second.Value.Day &&
                   first.Value.Month == second.Value.Month;
        }
    }
}