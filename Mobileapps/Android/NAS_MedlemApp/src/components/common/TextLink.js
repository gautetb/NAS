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
    color: 'white',
    fontSize: 20,
    fontWeight: '700',
    textDecorationLine: 'underline',
    paddingTop: 10,
    paddingBottom: 10,
  //  backgroundColor: 'white',
    marginLeft: 65,
    marginRight: 50,
    borderRadius: 25,
   // borderWidth: 3
  // backgroundColor: "black",
  },
  button: {
    marginTop: 5,
    alignSelf: 'center',
   // marginLeft: 25,
   // alignSelf: 'center',
    /*
    flex: 1,
    borderWidth: 3,
    borderColor: 'white',
    backgroundColor: "black",
    borderRadius: 25,
    marginTop: 5,
    marginLeft: 50,
    marginRight: 50,
    marginBottom: 5,
    opacity: 0.8,
    */
  },
 
};

export { TextLink };