using Pocos.OhioEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Support.DataConductor.ServerTests.TestHelpers
{
    public static class FlowFn
    {
        public static FlowDTO CreateAregi(this string flowFileName) =>
            new FlowDTO()
            {
                FileName = flowFileName,
                Reference = "AREGI005",
                Utility = "E",
                CreatedAt = DateTime.Now,
                ProcessedAt = DateTime.Now,
                FlowGroups = new List<FlowGroupDTO>
                {
                    new FlowGroupDTO()
                    {
                        Reference = "A01",
                        FlowItems = new List<FlowItemDTO>
                        {
                            new FlowItemDTO { Reference = "J0003", Value = "0123456789" },
                            new FlowItemDTO { Reference = "J0049", Value = "20200101"},
                            new FlowItemDTO { Reference = "X0016", Value = "A"},
                            new FlowItemDTO { Reference = "X0017", Value = ""},
                            new FlowItemDTO { Reference = "X0211", Value = ""}
                        },
                        FlowGroups = new List<FlowGroupDTO>
                        {
                            new FlowGroupDTO {
                                Reference = "A02",
                                FlowItems = new List<FlowItemDTO>
                                {
                                    new FlowItemDTO { Reference = "J0210", Value = "20200729" },
                                    new FlowItemDTO { Reference = "J0178", Value = "UKDC" },
                                    new FlowItemDTO { Reference = "J0048_MO", Value = "CYCLNHHR" }
                                }
                            }
                        }
                    }
                }
            };



        public static string GetDateTimeFrom(this string targetXml, string sourceXml)
        {
            string pattern = @"\d{14}";
            RegexOptions options = RegexOptions.Multiline;
            var dateTime = Regex.Matches(sourceXml, pattern, options).First().Value;
            var expected = Regex.Replace(targetXml, pattern, dateTime);
            return expected;
        }
    }
}
