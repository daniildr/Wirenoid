using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Wirenoid.WebApi.DbItems.Models
{
    public class LaunchedContainer
    {
        [Key]
        public Guid Id {  get; set; }

        [NotNull]
        public string ContainerId {  get; set; }

        [NotNull]
        public string ImageFullName { get; set; }

        [NotNull]
        public Uri Url {  get; set; }
    }
}
