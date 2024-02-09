import React, { Component } from 'react';
import CanvasJSReact from '@canvasjs/react-charts';
import Card from 'react-bootstrap/Card';
var CanvasJS = CanvasJSReact.CanvasJS;
var CanvasJSChart = CanvasJSReact.CanvasJSChart;
var updateInterval = 60000; 
export class Counter extends Component {
    constructor(props) {
        super(props);
     
        this.update = this.update.bind(this);
        this.state = {
           stockData: [], loading: true, 
            time: Date.now(),
            trend: [],
            company_plot:'XPP'
        };
        let dataArray = [];
        // Get the current date and time
        let currentDate = new Date();

        // Get the current hours
        
        let hh = currentDate.getHours()-5; //convert to local time 
        let mm = currentDate.getMinutes() - 40   // plot data from 10 min before
        for (let i =mm ; i <=mm+ 60; i++) {
            const dataPoint = {
                x: new Date(Date.UTC(2024, 2, 4, hh, i)),
                y:  null
            };

            dataArray.push(dataPoint);
        }
        this.options = {
            animationEnabled: true,
            zoomEnabled: true,
           
            title: {
                text: this.state.company_plot,
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
        this.load_data();
        this.update();

      
    }

    render() {
        return (
            <div style={{ display: 'flex', flexDirection: 'column' }} >
                <div>
                    <CanvasJSChart options={this.options} onRef={ref => this.chart = ref} />
                </div>
                <div style={{ display: 'flex', flexDirection: 'row', alignItems: 'flex-end', border:'black' }} onRef={ref => this.carditems = ref} >
                    
                    {this.state.trend.map((companystock, index) => (
                        <Card key={index} style={{ width: '10rem', margin: '2px',height:'auto' }}>
                            <Card.Body>
                                <Card.Text >
                                    <h6>{companystock.symbol}</h6>
                                    <p>Close: {companystock.close} Open:  {companystock.open}</p>
                                </Card.Text>
                            </Card.Body>
                        </Card>
                    ))}
                </div>
                <div>


                </div>

            
               </div>
        );
    }

    async update() {

        const responseTrend = await fetch('stockmockapi/trend', {
            headers: {
                'Content-Type': 'application/json',
            }
        });

        const dataTrend = await responseTrend.json();
        console.log("all data", dataTrend);
        this.setState({ trend: dataTrend });
        const length = this.options.data[0].dataPoints.length;
        var dataCurrent = null;
        for (let i = 0; i < dataTrend.length; i++) {
            console.log('symbol', 'coming from server: ',dataTrend[i].symbol, ', company to be plotted: ',this.state.company_plot);
            if (dataTrend[i].symbol == this.state.company_plot) {
                dataCurrent = dataTrend[i].close;
                this.state.stockData.push(dataTrend[i]);
                dataTrend.splice(i, 1);
                break;
            }

        }
        
    for(let i = 0; i <length; i++) {
            if (this.options.data[0].dataPoints[i].y === null) {
               
                this.options.data[0].dataPoints[i].y = dataCurrent;
                
                break;

    };
};
        this.chart.render();
        this.carditems.render();

        // Force a re-render by updating state
       
    }
    async load_data() {
 
        console.log('before loading ', this.options.data[0].dataPoints)
        const response = await fetch(`stockmockapi/close-price?company=${this.state.company_plot}`, {
            headers: {
                'Content-Type': 'application/json',
            }
        });
        const data = await response.json();
        console.log(data)
        this.setState({ stockData: data, loading: false });
        var length = data.length;
        this.options.title.text = this.state.company_plot;
        var currentDate= new Date()
        let hh = currentDate.getHours() - 5;     //convert to local time 
        let mm = currentDate.getMinutes() - 40
        // Find the first null data point and update it
        for (let i = 0; i < length; i++) {
            if (this.options.data[0].dataPoints[i].y === null) {

                this.options.data[0].dataPoints[i].y = data[i].close;
                this.options.data[0].dataPoints[i].x = new Date(Date.UTC(2024, 2, 4, hh, mm + i));

            }
        }
        for (let i = length; i < 60-length; i++) {
            if (this.options.data[0].dataPoints[i].y === null) {
                this.options.data[0].dataPoints[i].x = new Date(Date.UTC(2024, 2, 4, hh, mm+ i));

            }
        }

        this.chart.render();
        console.log('after loading points', this.options.data[0].dataPoints)
    }

}
