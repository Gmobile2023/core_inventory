using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Const
{
    public class ActivityLogTypeValue
    {
        /// <summary>
        /// 1.Phần tạo kho và kích hoạt kho
        /// </summary>
        public const string CreateStock = "Create-Stock";
        public const string EditStock = "Edit-Stock";
        public const string ApproveStock = "Approve-Stock";
        public const string ConfirmStock = "ConfirmStock";
        public const string ActiveStock = "Active-Stock";
        public const string CancelStock = "Cancel-Stock";
        public const string LockStock = "Lock-Stock";
        public const string UnLockStock = "Un-Lock-Stock";
        public const string AccountToStock = "Account-To-Stock";

        /// <summary>
        /// 2.Nhập serial vào kho
        /// </summary>
        public const string CreateSerial = "Create-Serial";
        public const string ConfirmSerial = "Confirm-Serial";
        public const string ApproveSerial = "Approve-Serial";
        public const string CancelSerial = "Cancel-Serial";

        /// <summary>
        /// 3.Điều chuyển serial
        /// </summary>
        public const string CreateTransferSerial = "Create-Transfer-Serial";
        public const string ApproveTransferSerial = "Approve-Transfer-Serial";
        public const string ConfirmTransferSerial = "Confirm-Transfer-Serial";
        public const string CancelTransferSerial = "Cancel-Transfer-Serial";       

        /// <summary>
        /// 4.Tạo số
        /// </summary>
        public const string CreateMobile = "Create-Mobile";
        public const string ApproveMobile = "Approve-Mobile";
        public const string ConfirmMobile = "Confirm-Mobile";
        public const string CancelMobile = "Cancel-Mobile";

        /// <summary>
        /// 5.Điều chuyển số
        /// </summary>
        public const string CreateTransferMobile = "Create-Transfer-Mobile";
        public const string ApproveTransferMobile = "Approve-Transfer-Serial";
        public const string ConfirmTransferMobile = "Confirm-Transfer-Serial";
        public const string CancelTransferMobile = "Cancel-Transfer-Serial";
        public const string ConvertPriceMobile = "Convert-Price-Mobile";
      
        /// <summary>
        /// 6.Kitting Serial và số
        /// </summary>
        public const string CreateKitting = "Create-Kitting";
        public const string ConfirmKitting = "Confirm-Kitting";
        public const string ApproveKitting = "Approve-Kitting";
        public const string CancelKitting = "Cancel-Kitting";

        /// <summary>
        /// 7.Thu hồi số
        /// </summary>
        public const string CreateRecoveMobile = "Create-Recove-Mobile";
        public const string ApproveRecoveMobile = "Approve-Recove-Mobile";
        public const string ConfirmRecoveMobile = "Confirm-Recove-Mobile";
        public const string CancelRecoveMobile = "Cancel-Recove-Mobile";

        /// <summary>
        /// 8.Thu hồi serial
        /// </summary>
        public const string CreateRecoveSerial = "Create-Recove-Serial";
        public const string ApproveRecoveSerial = "Approve-Recove-Serial";
        public const string ConfirmRecoveSerial = "Confirm-Recove-Serial";
        public const string CancelRecoveSerial = "Cancel-Recove-Serial";

        /// <summary>
        /// 9.Đối giá serial
        /// </summary>
        public const string CreatePriceSerial = "Create-Price-Serial";
        public const string ApprovePriceSerial = "Approve-Price-Serial";
        public const string ConfirmPriceSerial = "Confirm-Price-Serial";
        public const string CancelPriceSerial = "Cancel-Price-Serial";

        /// <summary>
        /// 10.Đổi giá của số
        /// </summary>
        public const string CreatePriceMobile = "Create-Price-Mobile";
        public const string ApprovePriceMobile = "Approve-Price-Mobile";
        public const string ConfirmPriceMobile = "Confirm-Price-Mobile";
        public const string CancelPriceMobile = "Cancel-Price-Mobile";

        /// <summary>
        /// 11.Bán serial
        /// </summary>
        public const string CreateSaleSerial = "Create-Sale-Serial";
        public const string ApproveSaleSerial = "Approve-Sale-Serial";
        public const string ConfirmSaleSerial = "Confirm-Sale-Serial";
        public const string CancelSaleSerial = "Cancel-Sale-Serial";

        /// <summary>
        /// 12.Bán số
        /// </summary>
        public const string CreateSaleMobile = "Create-Sale-Mobile";
        public const string ApproveSaleMobile = "Approve-Sale-Mobile";
        public const string ConfirmSaleMobile = "Confirm-Sale-Mobile";
        public const string CancelSaleMobile = "Cancel-Sale-Mobile";       
    }
}
