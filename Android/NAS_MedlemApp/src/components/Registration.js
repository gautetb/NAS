import React, { Component, Fragment } from 'react';
import { View, Text, ImageBackground, Image } from 'react-native';
import { Input, TextLink, Loading, Button, } from './common';
import axios from 'axios';
import qs from 'qs';
import deviceStorage from '../services/deviceStorage';

class Registration extends Component {
  constructor(props){
    super(props);
    this.state = {
      email: '',
      password: '',
      password_confirmation: '',
      error: '',
      loading: false
    };

    this.registerUser = this.registerUser.bind(this);
    this.onRegistrationFail = this.onRegistrationFail.bind(this);
  }

  registerUser() {
    const { email, password, password_confirmation } = this.state;
u
    this.setState({ error: '', loading: true });

    const axiosConfig = {
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      }
    }

    const postData = qs.stringify({
      Username: email,
      Password: password
    });

   axios.post("https://gautetester.azurewebsites.net/api/User", postData,
      axiosConfig
      )
    .then((response) => {
      console.log("Respons: "+ response.data);
      this.props.authSwitch();
    })
    .catch((error) => {
      console.log(Object.values(error));
      console.log(error.request.response);
      //console.log(response.data);
      this.onRegistrationFail(error.request.response);
    });
  }

  onRegistrationFail(errorResponse) {
    this.setState({
      error: errorResponse,
      loading: false
    });
  }

  render() {
    const { email, password, password_confirmation, error, loading } = this.state;
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

          <View style={section}>
            <Input
              secureTextEntry
              placeholder="confirm password"
              label="Confirm Password"
              value={password_confirmation}
              onChangeText={password_confirmation => this.setState({ password_confirmation })}
            />
          </View>

          <Text style={errorTextStyle}>
            {error}
          </Text>

          {!loading ?
            <Button onPress={this.registerUser}>
              Register
            </Button>
            :
            <Loading size={'large'} />
          }
        </View>
        <TextLink onPress={this.props.authSwitch}>
           Har du Brukskonto? Log in
        </TextLink>
        
        
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
  },
  section: {
    flexDirection: 'row',
    borderBottomWidth: 1,
    opacity: 0.9,
    backgroundColor: '#fff',
   // borderColor: '#ddd',
    marginLeft: 20,
    marginRight: 20,
    borderRadius: 15,
  },
  errorTextStyle: {
    alignSelf: 'center',
    fontSize: 18,
    color: 'red'
  },
  logoStyle: {
              
    width: '65%',
    height: '30%',
    margin: 10,
    alignSelf: 'center'
    

  },
  bgStyle: {
    width: '100%',
    height: '100%',
     
  },

};

export { Registration };