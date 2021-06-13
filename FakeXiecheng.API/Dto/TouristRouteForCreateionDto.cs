using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FakeXiecheng.API.ValidationAttributes;

namespace FakeXiecheng.API.Dto
{
	[TouristRouteTitleMustBeDifferentFromDescription]
	public class TouristRouteForCreationDto : TouristRouteForManipulationDto
	{
	}
}