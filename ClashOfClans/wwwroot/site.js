Vue.component("clash-clan-info",
    {
        props: {
            info: { type: Object },
            active: { type: Boolean, default: false }
        },
        template: `
<div class="block about" v-bind:class="{ active: active }">
    <div>
        <img v-bind:src="info.badgeUrls.small" alt="">
    </div>
    <div>
        <div>Всего очков: {{info.clanPoints}} / {{info.clanVersusPoints}}</div>
        <div>Серия побед: {{info.warWinStreak}}</div>
        <div>Выиграно войн: {{info.warWins}}</div>
        <div>Проиграно войн: {{info.warLosses}}</div>
        <div>Ничья: {{info.warTies}}</div>
        <div>Побед &divide; поражений: {{(info.warLosses === 0 ? 0 : info.warWins / info.warLosses).toFixed(3)}}</div>
        <div>Публичный варлог: {{info.isWarLogPublic ? "Да" : "Нет"}}</div>
        <div>Необходимо трофеев: {{info.requiredTrophies}}</div>
        <div>Расположение клана: {{info.location.name}}</div>
        <div>Соклановцы: {{info.members}}/50</div>
    </div>
</div>

`
    });

Vue.component("clash-clan-moto",
    {
        props: {
            description: {type: String},
            active: { type: Boolean, default: false }
        },
        template: `
<div class="block" v-bind:class="{ active: active }">
    <div>{{description}}</div>
</div>
`
    });

Vue.component("clash-clan-donation",
    {
        data: function () {
            return {
                count: 0
            }
        },
        template: '<a>!!!!!</a>'
    });

Vue.component("clash-warlog",
    {
        data: function () {
            return {
                count: 0
            }
        },
        template: '<a>!!!</a>'
    });

var app = new Vue({
    el: "#app",
    created() {
        this.fetchData();
    },
    data() {
        return {
            post: {
                warWins: 0,
                warLosses: 0,
                badgeUrls: { small: null },
                location: { name: null },
                description: ""
            },
            active: false
        }
    },
    methods: {
        fetchData() {
            fetch("api/clash").then(r => r.json().then(t => {
                this.post = t;
                this.active = true;
            }));
        }
    },
    template: `
<div>
<clash-clan-info v-bind:info="post" v-bind:active="active"></clash-clan-info>
<clash-clan-moto v-bind:description="post.description" v-bind:active="active"></clash-clan-moto>
<clash-clan-donation></clash-clan-donation>
<clash-warlog></clash-warlog>
</div>
`
})