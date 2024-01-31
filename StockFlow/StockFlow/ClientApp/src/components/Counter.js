import React, { Component } from 'react';

export class Counter extends Component {
    constructor(props) {
        super(props);
        this.state = {
            result: null,
            error: null,
        };
        this.url = new URL(`https://api.tinybird.co/v0/pipes/lt_pipe_6295.json`);
    }

    componentDidMount() {
        this.fetchData();
    }

    fetchData() {
        fetch(this.url, {
            headers: {
                Authorization: 'Bearer p.eyJ1IjogImJkNDhmYjhhLTIzOGUtNGZhYi1iYzRhLWVlNDBiNWM1YTQyNyIsICJpZCI6ICJhZWI4ZjE4Ni02OGUwLTRmYjAtYjFjYi0yNDAwZWIxYWQ2NDQiLCAiaG9zdCI6ICJldV9zaGFyZWQifQ.OJ2kmphiF1h4phGOtCfhk-L9BEUgLvjLcdYlBbjsr64', // Replace with your actual access token
            },
        })
            .then(response => response.json())
            .then(data => {
                if (!data.data) {
                    console.error(`There is a problem running the query: ${data}`);
                    this.setState({ error: `There is a problem running the query: ${data}` });
                } else {
                    console.table(data.data);
                    console.log('** Query columns **');
                    for (let column of data.meta) {
                        console.log(`${column.name} -> ${column.type}`);
                    }
                    this.setState({ result: data.data });
                }
            })
            .catch(error => {
                console.error(`Error fetching data: ${error.toString()}`);
                this.setState({ error: `Error fetching data: ${error.toString()}` });
            });
    }

    render() {
        const { result, error } = this.state;

        return (
            <div>
                <h1>Stock market</h1>
                {error && <p>Error: {error}</p>}
                {result && (
                    <div>
                        <h2>Data:</h2>
                        {/* Display your data in the desired format */}
                        <pre>{JSON.stringify(result, null, 2)}</pre>
                    </div>
                )}
            </div>
        );
    }
}
