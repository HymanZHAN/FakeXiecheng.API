using System;
using System.Text.RegularExpressions;

namespace FakeXiecheng.API.ResourceParameters
{
    public class TouristRouteResourceParameters
    {
        public string OrderBy { get; set; }
        public string Keyword { get; set; }
        public string RatingComparison { get; set; }
        public int? RatingValue { get; set; }

        private string _rating;

        public string Rating
        {
            get { return _rating; }
            set
            {
                _rating = value;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    Regex regex = new(@"([A-Za-z0-9\-]+)(\d+)");
                    Match match = regex.Match(value);
                    if (match.Success)
                    {
                        RatingComparison = match.Groups[1].Value;
                        RatingValue = Int32.Parse(match.Groups[2].Value);
                    }
                }
            }
        }

        public string Fields { get; set; }
    }
}