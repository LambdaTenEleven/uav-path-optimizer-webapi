using GeoCoordinatePortable;
using Riok.Mapperly.Abstractions;

namespace UavPathOptimization.WebAPI.DTO;

public record GeoCoordinateDto(double Latitude, double Longitude);

[Mapper]
public partial class GeoCoordinateMapper
{
    public partial GeoCoordinateDto GeoCoordinateToGeoCoordinateDto(GeoCoordinate geoCoordinate);

    public partial GeoCoordinate GeoCoordinateDtoToGeoCoordinate(GeoCoordinateDto geoCoordinateDto);
}