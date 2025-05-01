// Dummy Quick Action Functions
function addItem() {
    alert('Add New Item clicked!');
}

function createPurchaseOrder() {
    alert('Create Purchase Order clicked!');
}

// Chart.js - Stock In/Out Trends
const ctx = document.getElementById('stockChart').getContext('2d');
const stockChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
        datasets: [
            {
                label: 'Stock In',
                data: [120, 190, 300, 500, 200, 300],
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                borderColor: 'rgba(54, 162, 235, 1)',
                borderWidth: 2
            },
            {
                label: 'Stock Out',
                data: [80, 150, 250, 450, 150, 250],
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                borderColor: 'rgba(255, 99, 132, 1)',
                borderWidth: 2
            }
        ]
    },
    options: {
        responsive: true,
        plugins: {
            legend: {
                position: 'top',
            },
            title: {
                display: true,
                text: 'Stock In/Out Trends (Last 6 Months)'
            }
        }
    }
});
