﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaisyStudy.Models.VNPAY
{
    public class VNPayConfig
    {
        public static string? vnp_Url { get; set; }
        public static string? vnp_Returnurl { get; set; }
        public static string? vnp_TmnCode { get; set; }
        public static string? vnp_HashSecret { get; set; }
        public static string? querydr { get; set; }
    }
}
