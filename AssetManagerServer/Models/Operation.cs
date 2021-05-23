﻿using System;

namespace AssetManagerServer.Models
{
    public class Operation
    {
        public Operation()
        {
            
        }

        public Operation(Operation operation)
        {
            Id = operation.Id;
            AssetName = operation.AssetName;
            AssetTicker = operation.AssetTicker;
            AssetType = operation.AssetType;
            Datetime = operation.Datetime;
            Type = operation.Type;
            Price = operation.Price;
            UserId = operation.UserId;
            BrokerId = operation.BrokerId;
            AssetAnalyticId = operation.AssetAnalyticId;
        }
        
        public int Id { get; set; }
        public string AssetName { get; set; }
        public string AssetTicker { get; set; }
        public string AssetType { get; set; }
        public DateTime Datetime { get; set; }
        public int Type { get; set; }
        public float Price { get; set; }
        public int UserId { get; set; }
        public int BrokerId { get; set; }
        public int AssetAnalyticId { get; set; }
    }
}