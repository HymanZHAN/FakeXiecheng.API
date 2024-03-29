using System;
using System.Collections.Generic;
using Stateless;

namespace FakeXiecheng.API.Models
{
    public enum OrderStateEnum
    {
        Pending,
        Processing,
        Completed,
        Declined,
        Cancelled,
        Refund
    }

    public enum OrderStateTriggerEnum
    {
        PlaceOrder,
        Approve,
        Reject,
        Cancel,
        Return
    }

    public class Order
    {
        public Order()
        {
            StateMachineInit();
        }

        public Guid Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<LineItem> OrderItems { get; set; }

        public OrderStateEnum State { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public string TransactionMetaData { get; set; }

        StateMachine<OrderStateEnum, OrderStateTriggerEnum> _machine;

        public void ProcessPayment()
        {
            _machine.Fire(OrderStateTriggerEnum.PlaceOrder);
        }
        public void ApprovePayment()
        {
            _machine.Fire(OrderStateTriggerEnum.Approve);
        }
        public void RejectPayment()
        {
            _machine.Fire(OrderStateTriggerEnum.Reject);
        }

        private void StateMachineInit()
        {
            _machine = new StateMachine<OrderStateEnum, OrderStateTriggerEnum>
            (
                () => State, s => State = s
            );

            _machine.Configure(OrderStateEnum.Pending)
                .Permit(OrderStateTriggerEnum.PlaceOrder, OrderStateEnum.Processing)
                .Permit(OrderStateTriggerEnum.Cancel, OrderStateEnum.Cancelled);

            _machine.Configure(OrderStateEnum.Processing)
                .Permit(OrderStateTriggerEnum.Approve, OrderStateEnum.Completed)
                .Permit(OrderStateTriggerEnum.Reject, OrderStateEnum.Declined);

            _machine.Configure(OrderStateEnum.Declined)
                .Permit(OrderStateTriggerEnum.PlaceOrder, OrderStateEnum.Processing);

            _machine.Configure(OrderStateEnum.Completed)
                .Permit(OrderStateTriggerEnum.Return, OrderStateEnum.Refund);
        }
    }
}