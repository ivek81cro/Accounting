﻿using AccountingUI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingUI.Core.Services
{
    public interface ITravelOrdersEndpoint
    {
        Task<bool> PostLocoTravel(TravelOrdersLocoModel locoTravelOrder);
        Task<List<LocoCalculationModel>> GetLocoCalculations();
    }
}