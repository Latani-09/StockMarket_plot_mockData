import React, { Component } from 'react';
import CanvasJSReact from '@canvasjs/react-charts';

var CanvasJS = CanvasJSReact.CanvasJS;
var CanvasJSChart = CanvasJSReact.CanvasJSChart;
var updateInterval = 10000;
export class Counter extends Component {
    constructor(props) {
        super(props);
     
        this.update = this.update.bind(this);
        this.state = {
           forecasts: [], loading: true, 
            time: Date.now(),
        };

        this.options = {
            animationEnabled: true,
            title: {
                text: 'Stock Market Current Price',
            },
            axisX: {
                title: "time",
                gridThickness: 2,
                interval: 1,
                intervalType: "hour",
                valueFormatString: "hh",
                labelAngle: -20
            },
            axisY: {
                title: 'Sales (in USD)',
                prefix: '$',
            },
            data: [
                {
                    yValueFormatString: '$#,###',
                    xValueFormatString: 'hh',
                    type: 'spline',
                    dataPoints: [
                        { x: new Date(Date.UTC(2024, 2, 4, 1, 0)), y: 943 },
                        { x: new Date(Date.UTC(2024, 2, 4, 2, 0)), y: 942 },
                        { x: new Date(Date.UTC(2024, 2, 4, 3, 0)), y: 941 },
                        { x: new Date(Date.UTC(2024, 2, 4, 4, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 5, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 6, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 7, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 8, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 9, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 10, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 11, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 12, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 13, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 14, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 15, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 16, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 17, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 18, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 19, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 20, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 21, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 22, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 23, 0)), y: null },
                        { x: new Date(Date.UTC(2024, 2, 4, 24, 0)), y: null },
                     
                    ],
                },
            ],
        };
    }

    componentWillUnmount() {
        clearInterval(this.interval);
    }

    componentDidMount() {
        setInterval(this.update, updateInterval);

      
    }

    render() {
        return (
            <div>
                <CanvasJSChart options={this.options} onRef={ref => this.chart = ref} />
            
                <table className='table table-striped' aria-labelledby="tabelLabel" onRef={ref => this.table = ref}>
                <thead>
                    <tr>
                        <th>Date: {this.state.time}</th>
                        <th>Close (C)</th>
                    </tr>
                </thead>
                <tbody>
                    {this.state.forecasts.map((stock, index) => (
                        <tr key={index}>
                            <td>{stock.close}</td>
                            <td>{stock.time}</td>
                        </tr>
                    ))}
                </tbody>
                </table>
            </div>
        );
    }

    async update() {
        const response = await fetch('weatherforecast');
        const data = await response.json();
        console.log(data)
        this.setState({ forecasts: data, loading: false });
        var length = this.options.data[0].dataPoints.length;
        this.options.title.text = 'Last DataPoint Updated';

        // Find the first null data point and update it
        for (let i = 0; i < length; i++) {
            if (this.options.data[0].dataPoints[i].y === null) {
                console.log('forecast', this.state.forecasts[0])
                this.options.data[0].dataPoints[i].y = this.state.forecasts[0].close;
                break;
            }
        }

        this.chart.render();

        // Force a re-render by updating state
       
    }
    async populateStockData() {
        const response = await fetch('stock');
        const data = await response.json();
        console.log(data)
       

    }

}
