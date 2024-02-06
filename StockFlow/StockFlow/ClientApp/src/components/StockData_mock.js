import React, { Component } from 'react';
import CanvasJSReact from '@canvasjs/react-charts';
import Card from 'react-bootstrap/Card';
var CanvasJS = CanvasJSReact.CanvasJS;
var CanvasJSChart = CanvasJSReact.CanvasJSChart;
var updateInterval = 10000;
export class Counter extends Component {
    constructor(props) {
        super(props);
     
        this.update = this.update.bind(this);
        this.state = {
           stockData: [], loading: true, 
            time: Date.now(),
            trend: [],
            company:'XYZ'
        };
        let dataArray = [];
        let hh = 0;
        for (let i = 1; i <= 60; i++) {
            const dataPoint = {
                x: new Date(Date.UTC(2024, 2, 4, hh, i)),
                y: i <= 4 ? 943 - i : null
            };

            dataArray.push(dataPoint);
        }
        this.options = {
            animationEnabled: true,
            zoomEnabled: true,
           
            title: {
                text: 'Stock Market Current Price',
            },
            axisX: {
                
                    type: 'time',
                    title: 'Time',
                    
                    grid: {
                        display: true,
                        lineWidth: 2,
                    },
                    time: {
                        unit: 'minute',
                        stepSize: 1,
                        displayFormats: {
                            minute: 'mm',
                        },
                    },
                    angleLines: {
                        color: 'black',
                        lineWidth: 0.5,
                    },
               
                },
                
            
            axisY: {
                beginAtZero: true, // Set this to true to start the axis at 0
                title:  'Close (in USD)',
                prefix: '$',
            },
            data: [
                {
                    yValueFormatString: '$#,###',
                    xValueFormatString: 'mm',
                    type: 'spline',
                    dataPoints: dataArray,
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
            <div style={{ display: 'flex', flexDirection: 'column' }} >
                <div>
                    <CanvasJSChart options={this.options} onRef={ref => this.chart = ref} />
                </div>
                <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'flex-end', border:'black' }} onRef={ref => this.carditems = ref} >
                    <h2>Trending</h2>
                    {this.state.trend.map((companystock, index) => (
                        <Card key={index} style={{ width: '10rem', margin: '2px',height:'5rem' }}>
                            <Card.Body>
                                <Card.Text >
                                    <h6>{companystock.symbol}</h6>
                                    <p>Close: {companystock.close} </p>
                                </Card.Text>
                            </Card.Body>
                        </Card>
                    ))}
                </div>

            
               </div>
        );
    }

    async update() {
        const response = await fetch(`stockmockapi/close-price?company=${this.state.company}`, {
            headers: {
                'Content-Type': 'application/json',
            }
        });
        const data = await response.json();
        console.log(data)
        this.setState({ stockData: data, loading: false });
        var length = this.options.data[0].dataPoints.length;
        this.options.title.text = 'Close ';

        // Find the first null data point and update it
        for (let i = 0; i < length; i++) {
            if (this.options.data[0].dataPoints[i].y === null) {

                this.options.data[0].dataPoints[i].y = this.state.stockData[0].close;
                break;
            }
        }
        const responseTrend = await fetch('stockmockapi/trend', {
            headers: {
                'Content-Type': 'application/json',
            }
        });
        const dataTrend = await responseTrend.json();
        this.setState({ trend: dataTrend});
        this.chart.render();
        this.carditems.render();

        // Force a re-render by updating state
       
    }


}
