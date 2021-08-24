import React, { useCallback, useState } from 'react';
import logo from './logo.svg';
import './App.css';
import { Counter } from './Counter';

function App() {
  const [nextCounterIndex, setNextCounterIndex] = useState(1);
  const [blazorCounters, setBlazorCounters] = useState([]);
  const addBlazorCounter = () => {
    const index = nextCounterIndex;
    setNextCounterIndex(index + 1);
    setBlazorCounters(blazorCounters.concat([{
      title: `Counter ${index}`,
      incrementAmount: index,
      customObject: { StringValue: 'Hello!', IntegerValue: 42 },
    }]));
  };
  const removeBlazorCounter = () => {
    setBlazorCounters(blazorCounters.slice(0, -1));
  };
  const modifyParameters = () => {
    setBlazorCounters(blazorCounters.map((counter) => {
      return {
        ...counter,
        incrementAmount: counter.incrementAmount + 1,
        customObject: {
          StringValue: counter.customObject.StringValue + '!',
          IntegerValue: counter.customObject.IntegerValue - 1,
        }
      };
    }));
  };
  const logEventArgs = useCallback((eventArgs) => {
    console.log(eventArgs);
  }, []);

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          <button onClick={addBlazorCounter}>Add Blazor counter</button> &nbsp;
          <button onClick={removeBlazorCounter}>Remove Blazor counter</button> &nbsp;
          <button onClick={modifyParameters}>Modify parameters from JS</button>
        </p>

        {blazorCounters.map(counter =>
          <div key={counter.title}>
            <Counter
              title={counter.title}
              incrementAmount={counter.incrementAmount}
              customObject={counter.customObject}
              customCallback={logEventArgs}>
            </Counter>
          </div>
        )}

      </header>
    </div>
  );
}

export default App;
