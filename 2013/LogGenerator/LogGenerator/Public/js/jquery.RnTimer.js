(function(w, $) {
    $.widget('rn.RnTimer', {
        options: {
            debug: false,
            onAddTimer: null,
            onStart: null,
            onStop: null,
            onPause: null,
            onSetInterval: null,
            onStartAll: null,
            onStopAll: null,
            onPauseAll: null,
            onAddCallback: null,
            onRemoveCallback: null,
            onClearCallbacks: null,
            onChangeAllIntervals: null
        },
        // todo: error handler


        // ======================================================
        // Internal
        // ======================================================
        _timers: {},

        _create: function() {

        },

        _setOption: function(key, value) {
            this.options[key] = value;
        },
        
        _fireEvent: function (eventName, instance, eventArgs) {
            if (this.options[eventName]) {
                this.options[eventName].call(instance || this, eventArgs);
            }
        },

        destroy: function() {
            // Call the base destroy function.
            $.Widget.prototype.destroy.call(this);
        },
        
        _runTimer: function (timerName) {
            var timer = this.get(timerName);
            if (timer == null) {
                return;
            }

            // Check to see if the timer has been stopped
            if (!timer.running) {
                return;
            }

            // Fire off the timer
            var ctx = this;
            setTimeout(function () {
                timer.callCount++;
                ctx._debug('Number of calls: ' + timer.callCount + ' (' + timer.running + ')' + ' (Int: ' + timer.interval + ')' + ' (Timer: ' + timer.name + ')');

                $.each(timer.callbacks, function (idx, oCallback) {
                    if (oCallback.enabled) {
                        ctx._debug('firing callback [' + idx + '] on timer "' + timerName + '"');
                        oCallback.callCount++;
                        oCallback.fn.call(oCallback, timer.callCount, oCallback.args);
                        
                        if (oCallback.lifespan !== 0 && oCallback.lifespan == oCallback.callCount) {
                            // todo: allow a timer to expire after x ticks!
                            // todo: attach an event here
                            console.log('expire me!');
                        }
                    }
                });

                if (timer.running) {
                    ctx._runTimer(timerName);
                }
            }, this._timers[timerName].interval);
        },

        _debug: function (msg) {
            if (this.options.debug && w.console && w.console.log) {
                w.console.log(msg);
            }
        },


        // ======================================================
        // Single Timer
        // ======================================================
        exists: function(timerName) {
            if (timerName === undefined) {
                // I don't want timers being added with a NULL name
                return true;
            }

            return this._timers[timerName] !== undefined;
        },

        add: function(timerName, timerInterval, start, fnCallback, fnArgs, fnName) {
            // Ensure we are not adding the same timer twice
            if (this.exists(timerName)) {
                $.error('RnTimer: There is already a timer called "' + timerName + '"');
                return;
            }

            // Add our timer to _timers
            this._debug('adding new timer "' + timerName + '" (int:' + timerInterval + ')');
            this._timers[timerName] = {
                name: timerName,
                interval: timerInterval,
                running: start || false,
                paused: false,
                callCount: 0,
                callbacks: {}
            };

            // pop in our callback, if there is one
            if (fnCallback !== null && fnCallback !== undefined) {
                this.addCallback(timerName, (fnName || 'default'), fnCallback, fnArgs);
            }

            this._fireEvent('onAdd', this._timers[timerName]);

            // start the timer if we need to
            if (this._timers[timerName].running) {
                this._runTimer(timerName);
            }
        },
        
        setInterval: function (timerName, interval) {
            var timer = this.get(timerName);
            if (timer == null) {
                return;
            }

            var oldInterval = timer.interval;
            timer.interval = interval;
            this._debug('setting interval for timer "' + timerName + '" to ' + interval + 'ms');
            this._fireEvent('onSetInterval', timer, { oldInt: oldInterval, newInt: interval });
        },
        
        start: function (timerName) {
            var timer = this.get(timerName);
            if (!timer.running) {
                this._debug('starting timer "' + timerName + '"');
                timer.running = true;
                timer.paused = false;
                this._runTimer(timerName);
                this._fireEvent('onStart', timer);
            }
        },

        stop: function (timerName) {
            var timer = this.get(timerName);
            if (timer) {
                this._debug('stopping timer "' + timerName + '"');
                timer.running = false;
                this._fireEvent('onStop', timer);
            }
        },
        
        get: function (timerName) {
            if (this._timers[timerName] == undefined) {
                $.error('Timer "' + timerName + '" does not exist!');
                return null;
            }

            return this._timers[timerName];
        },

        pause: function (timerName, pauseTimeMs) {
            var timer = this.get(timerName);
            if (timer == null) {
                return;
            }

            // check to see if the timer is already paused
            if (timer.paused || !timer.running) {
                return;
            }

            var ctx = this;
            timer.paused = true;
            timer.running = false;
            this._debug('pausing timer "' + timerName + '" for ' + pauseTimeMs + 'ms');
            this._fireEvent('onPause', timer, { pauseTimeMs: pauseTimeMs });

            setTimeout(function () {
                ctx._debug('timer "' + timerName + '" has awoken from its sleep');
                if (timer.paused) {
                    timer.running = true;
                    timer.paused = false;
                    ctx._runTimer(timerName);
                }
            }, pauseTimeMs);
        },
        
        addCallback: function (timerName, fnName, fnCallback, fnArgs, lifespan) {
            var timer = this.get(timerName);
            if (timer == null) {
                return;
            }

            timer.callbacks[fnName] = {
                timerName: timerName,
                args: fnArgs,
                fn: fnCallback,
                enabled: true,
                callCount: 0,
                lifespan: lifespan || 0
            };

            this._debug('Added new callback (' + fnName + ') to timer (' + timerName + ')');
            this._fireEvent('onAddCallback', timer, { name: fnName });
        },
        
        removeCallback: function (timerName, fnName) {
            var timer = this.get(timerName);
            if (timer == null) {
                return;
            }

            if (timer.callbacks[fnName]) {
                delete timer.callbacks[fnName];
                this._debug('removing callback (' + fnName + ') from timer (' + timerName + ')');
                this._fireEvent('onRemoveCallback', timer, { name: fnName });
            }
        },

        clearCallbacks: function(timerName) {
            var timer = this.get(timerName);
            if (timer == null) {
                return;
            }

            this.stop(timerName);
            delete timer.callbacks;
            timer.callbacks = {};
            this._debug('removed all callbacks from timer "' + timerName + '"');
            this._fireEvent('onClearCallbacks', timer);
        },
        
        getCallbacks: function (timerName) {
            var timer = this.get(timerName);
            if (timer == null) {
                return null;
            }

            return timer.callbacks;
        },
        

        // ======================================================
        // All Timers
        // ======================================================
        startAll: function () {
            var ctx = this;
            $.each(this._timers, function (idx) {
                // todo - allow the ability to disable a timer
                ctx.start(idx);
            });
            this._fireEvent('onStartAll');
        },
        
        stopAll: function () {
            var ctx = this;
            $.each(this._timers, function (idx) {
                // todo - allow the ability to disable a timer
                ctx.stop(idx);
            });
            this._fireEvent('stoppedAll');
        },
        
        pauseAll: function (pauseTimeMs) {
            var ctx = this;
            $.each(this._timers, function (idx) {
                // todo - allow the ability to disable a timer
                ctx.pause(idx, pauseTimeMs);
            });
            this._fireEvent('onPauseAll', null, { pauseTimeMs: pauseTimeMs });
        },
        
        changeAllIntervals: function (timerInterval) {
            var ctx = this;
            $.each(this._timers, function (timerName) {
                ctx.setInterval(timerName, timerInterval);
            });
            this._fireEvent('onChangeAllIntervals', null, { newInt: timerInterval });
        }
        
    });

    $(w).RnTimer();
})(window, jQuery);


$(document).ready(function () {
    $(window).RnTimer('option', 'debug', true);

    // =====================================================================
    // Subscribe to RnTimer events
    // =====================================================================
    $(window).RnTimer('option', 'onAdd', function () {
        // Called when a new timer is added to "RnTimer"
        // "this" is bound to the new timer
        console.log('[EVENT] New timer added (' + this.name + ')');
    });

    $(window).RnTimer('option', 'onStart', function () {
        // Called when any timer is started
        // "this" is bound to the timer that was started
        console.log('[EVENT] Timer has been started (' + this.name + ')');
    });

    $(window).RnTimer('option', 'onStop', function () {
        // Called when any timer is stopped
        // "this" is bound to the timer that was stopped
        console.log('[EVENT] Timer has been stopped (' + this.name + ')');
    });
    
    $(window).RnTimer('option', 'onPause', function (args) {
        // Called when a timer is paused
        // "this" is bound to the timer that was paused
        // args = { pauseTimeMs: x }
        console.log('[EVENT] Timer has been paused (' + this.name + ') (' + args.pauseTimeMs + ' ms)');
    });
    
    $(window).RnTimer('option', 'onSetInterval', function (args) {
        // Called when a timer is paused
        // "this" is bound to the timer that was paused
        // args = { oldInt: x, newInt: x }
        console.log('[EVENT] Timer interval changed (' + this.name + ') (old:' + args.oldInt + ') (new:' + args.newInt + ')');
    });

    $(window).RnTimer('option', 'onAddCallback', function (args) {
        // Called when a new callback is added to a timer
        // "this" is bound to the timer that was paused
        // args = { name: x }
        console.log('[EVENT] Callback added to timer "' + this.name + '" (' + args.name + ')');
    });
    
    $(window).RnTimer('option', 'onRemoveCallback', function (args) {
        // Called when a new callback is removed from
        // "this" is bound to the timer that was paused
        // args = { name: x }
        console.log('[EVENT] Callback removed from timer "' + this.name + '" (' + args.name + ')');
    });
    
    $(window).RnTimer('option', 'onClearCallbacks', function () {
        // Called when a new callback is removed from
        // "this" is bound to the timer that was paused
        console.log('[EVENT] All callbacks removed from timer "' + this.name + '"');
    });


    $(window).RnTimer('option', 'onStartAll', function () {
        // Called after all timers have been started
        // "this" is bound to an anonomys function
        console.log('[EVENT] All timers have been started');
    });
    
    $(window).RnTimer('option', 'onStopAll', function () {
        // Called after all timers have been started
        // "this" is bound to an anonomys function
        console.log('[EVENT] All timers have been stopped');
    });
    
    $(window).RnTimer('option', 'onPauseAll', function (args) {
        // Called after all timers have been started
        // "this" is bound to an anonomys function
        // args = { pauseTimeMs: x }
        console.log('[EVENT] All timers have been paused for "' + args.pauseTimeMs + 'ms"');
    });
    
    $(window).RnTimer('option', 'onChangeAllIntervals', function (args) {
        // Called after all timer intervals have been changed
        // "this" is bound to an anonomys function
        // args = { newInt: x }
        console.log('[EVENT] All timers intervals have been changed to "' + args.newInt + 'ms"');
    });
    

    // =====================================================================
    // Timer controls
    // =====================================================================
    $('div#timerAdmin > button.add').click(function () {
        $(window).RnTimer('add', 'timer' + timerNo, 250);
        var html = [
            '<div class="timerControls timer{{timerNo}}" data-timer="timer{{timerNo}}">',
            '<span class="timer{{timerNo}}">Timer {{timerNo}}</span>',
            '<button class="start">Start</button>',
            '<button class="pause">Pause</button>',
            '<button class="stop">Stop</button>',
            '<button class="changeInterval">Set Interval</button>',
            '<button class="addCallback">Add Callback</button>',
            '<button class="clearCallbacks">Clear Callbacks</button>',
            '<button class="removeCallback">Remove Callback</button>',
            '</div>'
        ].join('\r\n').replace(/{{timerNo}}/g, timerNo++);

        $('div#allTimerControls').append(html);
    });

    $(document).on('click', 'button.start', function () {
        $(window).RnTimer('start', getTimerName(this));
    });
    
    $(document).on('click', 'button.stop', function () {
        $(window).RnTimer('stop', getTimerName(this));
    });
    
    $(document).on('click', 'button.changeInterval', function () {
        var intValue = prompt('New interval', 500);
        var interval = isNaN(intValue) ? 500 : intValue;
        $(window).RnTimer('setInterval', getTimerName(this), interval);
    });
    
    $(document).on('click', 'button.addCallback', function () {
        $(window).RnTimer('addCallback', getTimerName(this), 'fnCallback_' + fnAddedCount++, testCallback, { key: 'value', key2: 'value2' });
    });
    
    $(document).on('click', 'button.clearCallbacks', function () {
        $(window).RnTimer('clearCallbacks', getTimerName(this));
    });
    
    $(document).on('click', 'button.removeCallback', function () {
        var timerName = getTimerName(this);
        var cbs = $(window).RnTimer('getCallbacks', timerName);

        var ul = $('<ol >');
        $.each(cbs, function (name) {
            ul.append(
                $('<li />').append(
                    $('<a />')
                        .attr('href', '#')
                        .data('name', name)
                        .text(name)
                        .click(function () {
                            $(window).RnTimer('removeCallback', timerName, $(this).data('name'));
                            msgDiv.empty().removeClass('show');
                        }))
            );
        });

        msgDiv
            .addClass('show')
            .html('')
            .append('<div class="clearMsg">[close]</div>')
            .append('<p>Click on a callback to remove it from "<strong>' + timerName + '</strong>"</p>')
            .append(ul);
        
        $('html, body').animate({
            scrollTop: msgDiv.offset().top
        }, 1500);
    });
    
    $(document).on('click', 'button.pause', function () {
        var intValue = prompt('Pause time (ms)', 750);
        var interval = isNaN(intValue) ? 750 : intValue;
        $(window).RnTimer('pause', getTimerName(this), interval);
    });

    $(document).on('click', 'div#userInteraction > div.clearMsg', function () {
        msgDiv.removeClass('show').html('');
    });


    // =====================================================================
    // Global timer controls
    // =====================================================================
    $('button#startAll').click(function () {
        $(window).RnTimer('startAll');
    });

    $('button#stopAll').click(function () {
        $(window).RnTimer('stopAll');
    });

    $('button#pauseAll').click(function () {
        var intValue = prompt('Pause time (ms)', 3000);
        var interval = isNaN(intValue) ? 3000 : intValue;
        $(window).RnTimer('pauseAll', interval);
    });

    $('button#setAllInt').click(function () {
        var intValue = prompt('New interval', 500);
        var interval = isNaN(intValue) ? 500 : intValue;
        $(window).RnTimer('changeAllIntervals', interval);
    });


    // =====================================================================
    // Debugging controls
    // =====================================================================
    $('button#debugOn').click(function () {
        console.log('Turning debugging ON');
        $(window).RnTimer('option', 'debug', true);
    });
    
    $('button#debugOff').click(function () {
        console.log('Turning debugging OFF');
        $(window).RnTimer('option', 'debug', false);
    });

    $('button#clearConsole').click(function() {
        console.clear();
    });
    


    // =====================================================================
    // Setup the demo
    // =====================================================================
    $(window).RnTimer('add', 'timer1', 250);

    var testCallback = function (callCount, args) {
        // this = callback object
        // timerCallCount = timer call count
        // callbackArgsObj = arguments
        console.log('callback - (' + this.timerName + ') (ticks:' + callCount + ')');
    };
    var msgDiv = $('div#userInteraction');
    var fnAddedCount = 1;
    var timerNo = 2;
    
});

function getTimerName(button) {
    return $(button).parent().attr('data-timer');
}

// http://learn.jquery.com/plugins/advanced-plugin-concepts/
// http://learn.jquery.com/plugins/stateful-plugins-with-widget-factory/