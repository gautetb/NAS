import React from 'react';
import { View, Text, TouchableOpacity } from 'react-native';

const TextLink = ({ onPress, children }) => {
  const { button, text } = styles;
  return (
    <View style={{flexDirection: 'row'}}>
      <TouchableOpacity onPress={onPress} style={button}>
        <Text style={text}>
          {children}
        </Text>
      </TouchableOpacity>
    </View>
  );
};

const styles = {
  text: {
    alignSelf: 'center',
    textAlign: 'center',
    color: 'white',
    fontSize: 24,
    fontWeight: '700',
    textDecorationLine: 'underline',
    paddingTop: 5,
    paddingBottom: 5,
    // backgroundColor: 'white',
    //marginLeft: 55,
    //marginRight: 40,
    //borderRadius: 25,
   // borderWidth: 3
   //backgroundColor: "black",
  },
  button: {
    //flex: 1,
    //borderWidth: 3,
    //borderColor: 'white',
    //backgroundColor: "black",
    //borderRadius: 25,
    marginTop: 5,
    //marginLeft: 50,
    //marginRight: 50,
    //marginBottom: 5
  },
 
};

export { TextLink };