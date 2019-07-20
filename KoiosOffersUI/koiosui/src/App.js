import React from 'react';
import { OfferProvider } from './Components/OfferContext/OfferContext.js'
import OfferHeader from './Components/OfferHeader/OfferHeader'
import logo from './logo.svg';
import './App.css';

function App() {

  
  return (
    // <div className="App">
      
      <OfferProvider>
        {/* {console.log("a")} */}
        <OfferHeader/>
      </OfferProvider>
      /* <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.js</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header> */
    // </div>
  );
}

export default App;
