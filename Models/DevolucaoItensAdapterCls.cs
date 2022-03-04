namespace EloPedidos.Models
{
    public class DevolucaoItensAdapterCls
    {
        public long FT_PEDIDO_ITEM_ID { get; set; }
        public long CODPROD { get; set; }        
        public string DSCPROD { get; set; }
        public string QTDPROD { get; set; }
        public string QTDDEVOL { get; set; }
        public string QTDDEVOLNOW { get; set; }

        public DevolucaoItensAdapterCls()
        {

        }

        public DevolucaoItensAdapterCls(long _CODPROD, string _DSCPROD, string _QTDPROD, string _QTDDEVOL, string _QTDDEVOLNOW)
        {
            CODPROD = _CODPROD;
            DSCPROD = _DSCPROD;
            QTDPROD = _QTDPROD;
            QTDDEVOL = _QTDDEVOL;
            QTDDEVOLNOW = _QTDDEVOLNOW;
        }
    }
}