import React, { Component, Fragment } from 'react';
import { View, Text, ImageBackground, Image, FlatList,TouchableOpacity,  } from 'react-native';
import { Button, Loading } from '../components/common';
import axios from 'axios';
//import {Icon } from 'react-native-elements';
import AsyncStorage from '@react-native-community/async-storage';
import { tsBooleanKeyword } from '@babel/types';
//import console = require('console');

export default class Profile extends Component {
  constructor(props){
    super(props);
    this.state = {
      loading: true,
      user: '',
      error: '',
    }
  }

  async componentDidMount(){

    const value = await AsyncStorage.getItem('user');
    const item = JSON.parse(value);

    this.setState({
      loading: false,
      user: item
    });
        
  }


  render() {
    const { container, emailText, errorText, mskapText, userText, ikkegyldigText, gyldigText} = styles;
    const { loading, user, error} = this.state;

    console.log("Valid? "+user.ValidPayment)

    if (loading){
      return(
        <View style={container}>
          <Loading size={'large'} />
        </View>
      )
    } else {
        return(
          <ImageBackground 
      source={require('../assets/images/bg.png')}
      style={styles.bgStyle}
      >
      <Fragment>
         <Image   
        source={require('../assets/images/logo.png')}    
         style={styles.logoStyle}
         ></Image>
          <View style={container}>
                             
          {user ?
                <Text style={userText}> 
                {user.Name}
                </Text>
                  : 
                <Text style={errorText}>
                {error}
                </Text>
          }
          
          {user ?
                <Text style={emailText}>
               {user.Username}
                </Text>
                  :
                <Text style={errorText}>
                  {error}
                </Text>
          }
          
          {!user.ValidPayment ?
                <Image   
                source={require('../assets/images/Unvalid.png')}    
                 style={styles.unvalidlogoStyle}
                ></Image>
                 
                  :
                  
                <Image   
                source={require('../assets/images/Valid.png')}    
                style={styles.validlogoStyle}
                GYLDIG
                ></Image>
          }
          
          {!user.ValidPayment ?
                <Text style={ikkegyldigText}>
                IKKE GYLDIG
                </Text> 
                 
                  :
                  
                <Text style={gyldigText}>
                GYLDIG
                </Text>
          } 
                
          {user ?
                <Text style={mskapText}>
              
               {user.Membership}
                </Text> 
                  :
                  
                <Text style={errorText}>
                  {error}
                </Text>
          }
              
                   
           <Button onPress={this.props.deleteJWT}>
              Logg ut!
            </Button>
          </View>
          </Fragment>
      </ImageBackground>
      );
    }
  }
}

const styles = {
  container: {
    flex: 1,
    justifyContent: 'center'
  },
  
  subcontainer: {
    width: '80',
    height: '60',
    flex: 1,
    backgroundColor: "grey"
  },
  
  userText: {
    marginTop: '5%',  
    alignSelf: 'center',
    color: 'white',
    fontSize: 30,
    fontWeight: 'bold',
  },
  
  mskapText:{
    // marginTop: '5%',
    marginBottom: '3%',  
    alignSelf: 'center',
    color: 'white',
    fontSize: 30,
    fontWeight: 'bold', 
  },
    
  errorText: {
    alignSelf: 'center',
    fontSize: 18,
    color: 'red'
  },
  
  emailText: {
    alignSelf: 'center',
    fontSize: 18,
    color: 'white'
  },
  
  gyldigText: {
    borderWidth: 3,
    borderColor: 'green',
    backgroundColor: 'white',
    alignSelf: 'center',
    fontSize: 22,
    color: 'green',
    marginBottom: '5%',
    fontWeight: 'bold',
  },
  
  ikkegyldigText: {
    borderWidth: 3,
    borderColor: 'red',
    backgroundColor: 'white',
    alignSelf: 'center',
    fontSize: 22,
    color: 'red',
    marginBottom: '5%',
    fontWeight: 'bold',
  },
  
  logoStyle: {
    width: '65%',
    height: '32%',
    margin: 10,
    alignSelf: 'center',
    marginTop: '15%',
    marginBottom: '0%'
  },
  
  validlogoStyle: {   
    width: '30%',
    height: '24%',
    margin: 10,
    alignSelf: 'center',
    marginTop: '5%',
    marginBottom: '5%'
  },
  
  unvalidlogoStyle: {   
    width: '30%',
    height: '24%',
    margin: 10,
    alignSelf: 'center',
    marginTop: '5%',
    marginBottom: '5%'
  },
  
  bgStyle: {
    width: '100%',
    height: '100%',
     
  },
};
