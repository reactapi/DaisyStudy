﻿namespace DaisyStudy.Data;

public class NotificationImage
{
    public int ImageID { set; get; }
    public int NotificationID { set; get; }
    public Notification? Notification { set; get; }
    public string? ImagePath { set; get; }
    public long ImageFileSize { set; get; }
    public bool? IsDefault { set; get; }
}