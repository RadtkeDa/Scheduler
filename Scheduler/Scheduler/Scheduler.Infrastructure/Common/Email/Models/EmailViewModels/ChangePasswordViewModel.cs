﻿namespace Scheduler.Infrastructure.Common.Email.Models.EmailViewModels
{
    public class ChangePasswordViewModel : EmailBaseViewModel
    {
        public string Link { get; set; } = string.Empty;
    }
}
