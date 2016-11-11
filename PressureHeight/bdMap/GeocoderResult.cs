using PressureHeight.util;
using Windows.Data.Json;

namespace PressureHeight.bdMap
{
    public  class GeocoderResult
    {
        private AddressComponent componenta;
        /// <summary>
        ///结构化地址信息
        /// </summary>
      public string formattedAddress { get; set; }
        /// <summary>
        ///所在商圈信息
        /// </summary>
        public string business { get; set; }
        /// <summary>
        ///当前位置结合POI的语义化结构描述
        /// </summary>
        public string sematicDescription { get; set; }
        /// <summary>
        ///城市代码
        /// </summary>
        public int cityCode { get; set; }
        /// <summary>
        ///地址组成 
        /// </summary>
        public string componentS { get; set; }
        /// <summary>
        ///地址组成序列化 
        /// </summary>
        public AddressComponent componentA { get
            {
                if (componenta == null)
                {
                    if (componentJ != null)
                    {
                        componenta = JsonUtil.componentParse(componentJ);
                    }
                    else {
                        componenta = JsonUtil.componentParse(componentS);
                    }                   
                }
                return componenta;
            }}
        /// <summary>
        ///地址组成 
        /// </summary>
        public JsonObject componentJ { get; set; }
    }
}
