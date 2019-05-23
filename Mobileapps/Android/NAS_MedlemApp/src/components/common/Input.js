import React from 'react';
import { View, TextInput, Text } from 'react-native';

const Input = ({ label, value, onChangeText, placeholder, secureTextEntry, multiline, numberOfLines }) => {
  const {inputStyle, labelStyle, containerStyle } = styles;

  return (
    <View style={containerStyle}>
      <Text style={labelStyle}>{label}</Text>
      <TextInput
        secureTextEntry={secureTextEntry}
        placeholder={placeholder}
        value={value}
        onChangeText={onChangeText}
        autoCorrect={false}
        multiline={multiline}
        numberOfLines={numberOfLines}
        style={inputStyle}
      />
    </View>
  );
};

const styles = {
  containerStyle: {
    height: 40,
    flex: 1,
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: "black",
    borderRadius: 30,
   // color: 'white',
    borderColor: 'white',
    borderWidth: 1,
   // opacity: 0.8,
  },
  labelStyle: {
    fontSize: 10,
    paddingLeft: 20,
    flex: 1,
    color: 'white',
  },
  inputStyle: {
    color: 'white',
    paddingRight: 5,
    paddingLeft: 5,

    fontSize: 18,
    lineHeight: 23,
    flex: 3,
   // backgroundColor: 'white'
  
  }
};

export { Input };