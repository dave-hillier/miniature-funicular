import React from 'react';
import logo from './logo.svg';
import './App.css';
import { getToken, generateLoginUrl } from './login';

const App: React.FC = () => {

  const urlParams = new URLSearchParams(window.location.search);
  const error = urlParams.get('error');
  const code = urlParams.get('code');
  if (error) {
    console.log("error");
  } else if (!code) {
    generateLoginUrl(`https://${window.location.host}/login`).then(
      result => window.location.href = result.loginUrl
    );
  } else {
    getToken(code).then(result => {
      console.log("access " + result.accessToken);
      //fetch()
      const headers = {
        'Accept': 'application/json, text/plain',
        'Content-Type': 'application/json;charset=UTF-8'
      }
      fetch('https://localhost:5001/api/properties/', {
        method: 'POST',
        headers: new Headers({
          'Authorization': 'Bearer ' + result.accessToken,
          ...headers
        }),
        //mode: 'no-cors',
        body: JSON.stringify({
          'name': {
            'en': 'Hotel 1',
          },
          'category': 'Hotel Category'
        })
      }).then(result => {
        console.log(result);
      });
    });
  }

  // TODO:

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.tsx</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>
  );
}

export default App;
