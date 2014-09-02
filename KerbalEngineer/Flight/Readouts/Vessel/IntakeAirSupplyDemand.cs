﻿// 
//     Kerbal Engineer Redux
// 
//     Copyright (C) 2014 CYBUTEK
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

#region Using Directives

using System.Linq;

#endregion

namespace KerbalEngineer.Flight.Readouts.Vessel
{
    public class IntakeAirSupplyDemand : ReadoutModule
    {
        #region Fields

        private double demand;
        private double supply;

        #endregion

        #region Constructors

        public IntakeAirSupplyDemand()
        {
            this.Name = "Intake Air (D/S)";
            this.Category = ReadoutCategory.GetCategory("Vessel");
            this.HelpString = string.Empty;
            this.IsDefault = true;
        }

        #endregion

        #region Methods: public

        public static double GetDemand()
        {
            var demand = 0.0;
            foreach (var part in FlightGlobals.ActiveVessel.Parts)
            {
                if (part.Modules.Contains("ModuleEngines"))
                {
                    var engine = part.Modules["ModuleEngines"] as ModuleEngines;
                    if (engine.isOperational)
                    {
                        demand += engine.propellants
                            .Where(p => p.name == "IntakeAir")
                            .Sum(p => p.currentRequirement);
                    }
                }
                if (part.Modules.Contains("ModuleEnginesFX"))
                {
                    var engine = part.Modules["ModuleEngines"] as ModuleEnginesFX;
                    if (engine.isOperational)
                    {
                        demand += engine.propellants
                            .Where(p => p.name == "IntakeAir")
                            .Sum(p => p.currentRequirement);
                    }
                }
            }
            return demand;
        }

        public static double GetSupply()
        {
            return FlightGlobals.ActiveVessel.Parts
                .Where(p => p.Resources.Contains("IntakeAir"))
                .Sum(p => p.Resources["IntakeAir"].amount);
        }

        public override void Draw()
        {
            this.DrawLine(this.demand.ToString("F4") + " / " + this.supply.ToString("F4"));
        }

        public override void Update()
        {
            this.demand = GetDemand();
            this.supply = GetSupply();
        }

        #endregion
    }
}