namespace PressureHeight.bdMap
{
    public class AddressComponent
    {
        /// <summary>
        ///国家 
        /// </summary>
        public string country { get; set; }
        /// <summary>
        ///国家代码 
        /// </summary>
        public int countryCode { get; set; }
        /// <summary>
        ///省名
        /// </summary>
        public string province { get; set; }
        /// <summary>
        ///城市名
        /// </summary>
        public string city { get; set; }
        /// <summary>
        ///区县名
        /// </summary>
        public string district { get; set; }
        /// <summary>
        ///行政区划代码
        /// </summary>
        public string adcode { get; set; }
        /// <summary>
        ///街道名
        /// </summary>
        public string street { get; set; }
        /// <summary>
        ///街道门牌
        /// </summary>
        public string streetNumber { get; set; }
        /// <summary>
        ///和当前坐标的方向，有门牌号的时候返回数据
        /// </summary>
        public string direction { get; set; }
        /// <summary>
        ///和当前坐标的距离，有门牌号的时候返回数据
        /// </summary>
        public string distance { get; set; }
    }
}
