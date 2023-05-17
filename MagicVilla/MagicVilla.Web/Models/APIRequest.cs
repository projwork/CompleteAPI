﻿using MagicVilla.Utility;
using static MagicVilla.Utility.SD;

namespace MagicVilla.Web.Models
{
    public class APIRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }
    }
}
