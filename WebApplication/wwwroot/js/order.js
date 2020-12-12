"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

const confirmOrderButton = document.getElementById("confirmOrderButton");
if (confirmOrderButton != null) {
    //Disable send button until connection is established
    confirmOrderButton.disabled = true;
}

connection.on("ReceiveMessage", function (jsonOrders) {
    const orders = JSON.parse(jsonOrders);
    const manageOrdersTable = document.getElementById("manageOrdersTable");

    for (let i = 0; i < orders.length; i++) {
        let newOrderRow = manageOrdersTable.insertRow(-1);

        let productNameCell = newOrderRow.insertCell(0);
        let priceCell = newOrderRow.insertCell(1);
        let amountCell = newOrderRow.insertCell(2);
        let phoneCell = newOrderRow.insertCell(3);
        let reviewButtonCell = newOrderRow.insertCell(4)

        productNameCell.innerHTML = orders[i]['Painting']['Name']
        priceCell.innerHTML = orders[i]['Painting']['Price']
        amountCell.innerHTML = orders[i]['Amount']
        phoneCell.innerHTML = orders[i]['PhoneNumber']
        
        
        const reviewButton = document.createElement("a");
        reviewButton.innerHTML = "Review"
        reviewButton.setAttribute("href","/Administration/ReviewOrder?orderId="+orders[i]['Id'])
        reviewButton.setAttribute("class","btn btn-outline-success")
        reviewButtonCell.appendChild(reviewButton)

    }
});

connection.start().then(function () {
    if (confirmOrderButton != null) {
        confirmOrderButton.disabled = false;
    }
}).catch(function (err) {
    return console.error(err.toString());
});
