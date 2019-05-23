import React, { Component, Fragment } from 'react';
import { View, Text, ImageBackground, Image} from 'react-native';
import { Input, TextLink, Loading, Button } from './common';
import axios from 'axios';
import qs from 'qs';
import deviceStorage from '../services/deviceStorage';

class Login extends Component {
  constructor(props){
    super(props);
    this.state = {
      email: '',
      password: '',
      error: '',
      loading: false
    };

    this.loginUser = this.loginUser.bind(this);
    this.onLoginFail = this.onLoginFail.bind(this);
  }

  loginUser() {
    const { email, password} = this.state;

    const postData = qs.stringify({
      grant_type: 'password',
      username: email,
      password: password
    });

    const axiosConfig = {
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      }
    }

    this.setState({ error: '', loading: true });

    axios.post("https://gautetester.azurewebsites.net/token",
      postData, axiosConfig
    )
    .then((response) => {
      this.onLoginSuccess(response.data.access_token)
    })
    .catch((error) => {
      console.log(error);
      this.onLoginFail();
    });
  }

  onLoginFail() {
    this.setState({
      error: 'Login Failed',
      loading: false
    });
  }

  onLoginSuccess(token) {
    const axiosConfig = {
      headers: {
        'Authorization': 'Bearer ' + token
      }
    }
    axios.get('https://gautetester.azurewebsites.net/api/User', 
      axiosConfig)
    .then((response) => {

      
      deviceStorage.saveKey("user", response.data);
      this.props.newJWT(response.data);

    }).catch((error) => {
      this.setState({
        error: 'Error retrieving data',
        loading: false
      });
    });
  }

  render() {
    const { email, password, error, loading } = this.state;
    const { form, section, errorTextStyle } = styles;

    return (
      <ImageBackground 
      source={require('../assets/images/bg.jpg')}
      style={styles.bgStyle}
      >
      <Fragment>
       
        <Image   
        source={require('../assets/images/logo.png')}    
         style={styles.logoStyle}
         ></Image>
         
        <View style={form}>
          <View style={section}>
            <Input
              placeholder="user@email.com"
              label="Email"
              value={email}
              onChangeText={email => this.setState({ email })}
            />
          </View>

          <View style={section}>
            <Input
              secureTextEntry
              placeholder="password"
              label="Password"
              value={password}
              onChangeText={password => this.setState({ password })}
            />
          </View>

          <Text style={errorTextStyle}>
            {error}
          </Text>

          {!loading ?
            <Button onPress={this.loginUser}>
              Login
            </Button>
            :
            <Loading size={'large'} />
          }
          <TextLink onPress={this.props.authSwitch}>
                 
            Ny Bruker? Register
        </TextLink>
        </View>
                
      </Fragment>
      </ImageBackground>
    );
  }
}
const styles = {
  form: {
    width: '100%',
   // borderTopWidth: 1,
   // borderColor: '#ddd',
   // borderColor: '',
  },
  section: {
    flexDirection: 'row',
    borderBottomWidth: 1,
    opacity: 0.9,
    backgroundColor: '#fff',
    //marginTop: 2,
   // borderColor: '#ddd',
  // borderColor: 'mediumpurple',
    marginLeft: 20,
    marginRight: 20,
  
    borderRadius: 15,
  },
  errorTextStyle: {
    alignSelf: 'center',
    fontSize: 18,
    color: 'red',
    backgroundColor: 'grey',

  },
  logoStyle: {
              
    width: '65%',
    height: '32%',
    margin: 10,
    alignSelf: 'center'
    

  },
  bgStyle: {
    width: '100%',
    height: '100%',
     
  },


};
export { Login };