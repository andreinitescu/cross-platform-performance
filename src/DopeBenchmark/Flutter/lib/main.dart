import 'dart:math';
import 'dart:async';
import 'package:flutter/material.dart';
import 'rand.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Dope Test',
      theme: ThemeData(
          scaffoldBackgroundColor: Colors.black),
      home: MyHomePage(title: 'Dope Test', key: null,),
    );
  }
}

class MyHomePage extends StatefulWidget {
  const MyHomePage({Key? key, required this.title}) : super(key: key);

  final String title;

  @override
  _MyHomePageState createState() => _MyHomePageState();
}

class _Label {
  int id = 0;
  Color? color;
  double top = 0;
  double left = 0;
  double rotate = 0;
}

class _MyHomePageState extends State<MyHomePage> {
  final List<_Label> _labels = List.empty(growable: true);
  double _width = 0;
  double _height = 0;
  bool _started = false;
  bool _startedOnce = false;
  static const int _max = 100;
  final Stopwatch _sw = Stopwatch();
  int? _prevElapsed;
  int _processed = 0;
  int _prevCount = 0;
  double _accum = 0;
  int _accumN = 0;
  String _dopes = '';

  Random2 rand = Random2(0);

  void loop() {
    var label = _Label();
    label.color = Color.fromARGB(
        255,
        (rand.nextDouble() * 255).round(),
        (rand.nextDouble() * 255).round(),
        (rand.nextDouble() * 255).round());
    label.top = rand.nextDouble() * _height;
    label.left = rand.nextDouble() * _width;
    label.rotate = rand.nextDouble() * pi * 2;
    label.id = _processed;
    _processed++;

    if (_processed > _max) {
      _labels.removeAt(0);

      if (_prevElapsed == null) {
        _prevElapsed = _sw.elapsedMilliseconds;
        _prevCount = _processed;
      }

      var diff = _sw.elapsedMilliseconds - _prevElapsed!;

      if (diff > 500) {
        _prevElapsed = _sw.elapsedMilliseconds;
        var val = (_processed - _prevCount) / diff * 1000;
        _dopes = '${val.toStringAsFixed(2)} Dopes/s';
        _accum += val;
        _accumN++;
        _prevElapsed = _sw.elapsedMilliseconds;
        _prevCount = _processed;
      }
    }
    _labels.add(label);

    setState(() {
      if (_started) {
        Timer.run(loop);
      }
      else {
        _dopes = '${(_accum / _accumN).toStringAsFixed(2)} Dopes/s (AVG)';
      }
    });
  }

  void _buttonClick() {
    setState(() {
      if (!_started) {
        _started = _startedOnce = true;
        _dopes = 'Warming up...';
        _sw.start();
        _prevElapsed = null;
        _accum = 0;
        _accumN = 0;
      }
      else {
        _started = false;
      }

      Timer.run(loop);
    });
  }

  @override
  Widget build(BuildContext context) {
    if (_width == null) {
      _width = MediaQuery
          .of(context)
          .size
          .width;
      _height = MediaQuery
          .of(context)
          .size
          .height;
    }

    var children = <Widget>[];

    _labels.forEach((element) {
      children.add(Transform(
        transform: Matrix4.translationValues(element.left, element.top, 0)
          ..rotateZ(element.rotate),
        key: ValueKey(element.id),
        child: Text('Dope', style: TextStyle(color: element.color)),
      ));
    });

    if (_startedOnce) {
      children.add(
          Align(
              alignment: Alignment.topCenter,
              child: Padding(
                  padding: const EdgeInsets.all(25.0),
                  child:
                  Container(
                      padding: const EdgeInsets.all(7.0),
                      color: Colors.red,
                      child: Text('$_dopes', style: const TextStyle(
                          color: Colors.white, backgroundColor: Colors.red))
                  )
              )
          ));
    }

    return Scaffold(
        body:
        Stack(
          children: children,
        ),

        floatingActionButton: TextButton(
          onPressed: _buttonClick,
          style: ButtonStyle(
              backgroundColor: MaterialStateProperty.all(
                  !_started ? Colors.green : Colors.red),
              foregroundColor: MaterialStateProperty.resolveWith(
                      (state) => Colors.white)),
          child: !_started ? Text('@ Start') : Text('@ Stop'),
        ),
        floatingActionButtonLocation: FloatingActionButtonLocation.centerFloat
    );
  }
}