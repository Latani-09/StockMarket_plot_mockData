import React, { Component } from 'react';

export class FetchData extends Component {
    static displayName = FetchData.name;

    constructor(props) {
        super(props);
        this.state = { forecasts: [], loading: true ,time:Date.now()};
    }



    componentWillUnmount() {
        clearInterval(this.interval);
    }
    componentDidMount() {
        this.interval = setInterval(() => {
            this.setState({ time: Date.now() });
            this.populateWeatherData();
        }, 60000); // Update every minute (60,000 milliseconds)
        //this.populateWeatherData(); // Initial fetch
        this.populateStockData();
    }

    static renderForecastsTable(forecasts,time) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Date: {time}</th>
                        <th>Close (C)</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map((stock, index) => (
                        <tr key={index}>
                            <td>{stock.close}</td>
                            <td>{stock.time}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchData.renderForecastsTable(this.state.forecasts,this.state.time);

        return (
            <div>
                <h1 id="tabelLabel" >stock market details </h1>

                {contents}
            </div>
        );
    }

    async populateWeatherData() {
        const response = await fetch('stock');
        const data = await response.json();
        console.log(data)
        this.setState({ forecasts: data, loading: false });
    }
    async populateStockData() {
        ////const response = await fetch('stock');
        //const data = await response.json();
       // console.log(data)
        const responsestock = await fetch(" https://query2.finance.yahoo.com/v7/finance/quote?symbols=TSLA", {
            method:"Get",
            headers: {

                'User-Agent': 'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36'
            }
        })
            .then(response => response.json())
            .then(data => console.log(JSON.stringify(data)));
        console.log(responsestock)
      
    }
}
