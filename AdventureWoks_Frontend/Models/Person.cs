﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AdventureWoks_Frontend.Models
{
    public class ApiResponse
    {
        [JsonPropertyName("$id")]
        public string? Id { get; set; }

        public Data? Data { get; set; }

        public MetaData? MetaData { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("$id")]
        public string? Id { get; set; }

        [JsonPropertyName("$values")]
        public List<Person>? Persons { get; set; }
    }

    public class Person
    {
        [JsonPropertyName("$id")]
        public string? Id { get; set; }

        public int BusinessEntityId { get; set; }

        [Required]
        public string? PersonType { get; set; }
        [StringLength(8, MinimumLength = 1)]
        public string? Title { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [StringLength(10, MinimumLength = 1)]
        public string? Suffix { get; set; }

        [Required]
        public int EmailPromotion { get; set; }

        [Display(Name = "Last Updated On")]
        [DataType(DataType.Date)]
        public DateTime ModifiedDate { get; set; }

        public EmailAddresses? EmailAddresses { get; set; }

        public PhoneNumbers? PhoneNumbers { get; set; }
    }

    public class EmailAddresses
    {
        [JsonPropertyName("$id")]
        public string? Id { get; set; }

        [JsonPropertyName("$values")]
        public List<string>? EmailAddress { get; set; }
    }

    public class PhoneNumbers
    {
        [JsonPropertyName("$id")]
        public string? Id { get; set; }

        [JsonPropertyName("$values")]
        public List<string>? PhoneNumber { get; set; }
    }

    public class MetaData
    {
        [JsonPropertyName("$id")]
        public int Id { get; set; }

        public int Total { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }
    }

}