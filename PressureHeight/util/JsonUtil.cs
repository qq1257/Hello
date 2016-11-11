using PressureHeight.bdMap;
using Windows.Data.Json;

namespace PressureHeight.util
{
    public class JsonUtil
    {
        public static GeocoderResult gpsToAddress(string body)
        {
            GeocoderResult geocoder = new GeocoderResult();
            JsonObject jsonObject = JsonObject.Parse(body);
            int status = (int)jsonObject.GetNamedNumber("status");
            if (status == 0)
            {
                JsonObject result = jsonObject.GetNamedObject("result");
                //JsonObject location = result.GetNamedObject("location");
                //{
                //    double latitude = location.GetNamedNumber("lat");
                //    double longitude = location.GetNamedNumber("lng");
                //}
                //结构化地址信息
                geocoder.formattedAddress = result.GetNamedString("formatted_address");
                //所在商圈信息
                geocoder.business = result.GetNamedString("business");
                //当前位置结合POI的语义化结构描述
                geocoder.sematicDescription = result.GetNamedString("sematic_description");
                //城市代码
                geocoder.cityCode = (int)result.GetNamedNumber("cityCode");
                geocoder.componentJ = result.GetNamedObject("addressComponent");
                //JsonArray pois = result.GetNamedArray("pois");
                //JsonArray poiRegions = result.GetNamedArray("poiRegions");
            }
            return geocoder;
        }

        public static AddressComponent componentParse(JsonObject addressComponent)
        {
            AddressComponent componetn = new AddressComponent();
            //国家
            componetn.country = addressComponent.GetNamedString("country");
            //国家代码
            componetn.countryCode = (int)addressComponent.GetNamedNumber("country_code");
            //省名
            componetn.province = addressComponent.GetNamedString("province");
            //城市名
            componetn.city = addressComponent.GetNamedString("city");
            //区县名
            componetn.district = addressComponent.GetNamedString("district");
            //行政区划代码
            componetn.adcode = addressComponent.GetNamedString("adcode");
            //街道名
            componetn.street = addressComponent.GetNamedString("street");
            //街道门牌
            componetn.streetNumber = addressComponent.GetNamedString("street_number");
            //和当前坐标的方向，有门牌号的时候返回数据
            componetn.direction = addressComponent.GetNamedString("direction");
            //和当前坐标的距离，有门牌号的时候返回数据
            componetn.distance = addressComponent.GetNamedString("distance");
            return componetn;
        }
        public static AddressComponent componentParse(string component)
        {
           return componentParse(JsonObject.Parse(component));
        }
    }
}
