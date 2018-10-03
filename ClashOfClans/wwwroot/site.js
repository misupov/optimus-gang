Vue.component("clash-clan-info",
    {
        props: ["info"],
        template: `
<div class="block about">
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
        props: ["info"],
        template: `
<div class="block">
    <div>{{info.description}}</div>
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
                location: { name: null }
            }
        }
    },
    methods: {
        fetchData() {
            fetch("api/clash").then(r => r.json().then(t => this.post = t));
        }
    },
    template: `
<div>
<clash-clan-info v-bind:info="post"></clash-clan-info>
<clash-clan-moto v-bind:info="post"></clash-clan-moto>
</div>
`
})